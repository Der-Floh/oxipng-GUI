using System.ComponentModel;

using OxipngGui.Models;
using OxipngGui.Utils;

namespace OxipngGui.Services;

/// <summary>
/// Optimizes one file with oxipng. oxipng truncates and rewrites files in place, so it always writes to a temp file
/// next to the target, which replaces the target only once it's complete. Stopping oxipng never damages an original.
/// </summary>
public sealed class OxipngOptimizer(IOxipngBinary oxipng, IProcessRunner processRunner) : IPngOptimizer
{
    private const string TempPrefix = "~oxipng-";
    private const string TempExtension = ".tmp";
    private const string BackupExtension = ".bak";
    private const int TempSuffixLength = 8;
    private const int CommitAttempts = 5;
    private const int Win32ErrorCodeMask = 0xFFFF;
    private const int ErrorSharingViolation = 32;
    private const int ErrorLockViolation = 33;
    private const int ErrorUnableToRemoveReplaced = 1175;
    private const string ReadOnlyMessage = "The file is read-only.";
    private const string ChangedMessage = "The file changed while it was being optimized.";
    private const string UnknownErrorMessage = "oxipng reported an unknown error.";
    private static readonly TimeSpan CommitRetryDelay = TimeSpan.FromMilliseconds(200);

    private readonly IOxipngBinary _oxipng = oxipng ?? throw new ArgumentNullException(nameof(oxipng));
    private readonly IProcessRunner _processRunner = processRunner ?? throw new ArgumentNullException(nameof(processRunner));

    public FileResult Optimize(PngJob job, OptimizationOptions options, int threads, CancellationToken cancellationToken = default)
    {
        if (job is null)
            throw new ArgumentNullException(nameof(job));
        if (options is null)
            throw new ArgumentNullException(nameof(options));

        try
        {
            return OptimizeFile(job, options, threads, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            return FileResult.Stopped(job.File);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or Win32Exception)
        {
            return FileResult.Failed(job.File, ex.Message);
        }
    }

    private FileResult OptimizeFile(PngJob job, OptimizationOptions options, int threads, CancellationToken cancellationToken)
    {
        var source = job.File.FullPath;
        var before = Snapshot(source);
        if (IsReadOnly(job.TargetPath))
            return FileResult.Failed(job.File, ReadOnlyMessage);

        var tempPath = PrepareTempPath(job.TargetPath);
        try
        {
            var report = RunOxipng(source, tempPath, options, threads, cancellationToken);
            if (!report.Succeeded)
                return FileResult.Failed(job.File, report.Error ?? UnknownErrorMessage);

            if (Snapshot(source) != before)
                return FileResult.Failed(job.File, ChangedMessage);

            return Commit(job, tempPath, before.Length, options.ForceWrite);
        }
        finally
        {
            // If the original is gone, the temp file holds the only copy of the image, so it stays.
            if (File.Exists(source))
                FileHelper.TryDelete(tempPath);
        }
    }

    private OxipngReport RunOxipng(string inputPath, string tempPath, OptimizationOptions options, int threads, CancellationToken cancellationToken)
    {
        var arguments = OxipngArguments.ForFile(options, threads, inputPath, tempPath);
        var result = _processRunner.Run(_oxipng.EnsureAvailable(), arguments, cancellationToken);
        return OxipngReport.Parse(result.StandardOutput)
            ?? throw new IOException($"oxipng stopped without a result (exit code {result.ExitCode}).");
    }

    private static FileResult Commit(PngJob job, string tempPath, long originalSize, bool forceWrite)
    {
        var optimizedSize = new FileInfo(tempPath).Length;
        var isRewritten = optimizedSize < originalSize || forceWrite;
        if (job.OverwritesOriginal && !isRewritten)
            return FileResult.AlreadyOptimized(job.File, originalSize);

        FlushToDisk(tempPath);
        ReplaceWithRetries(tempPath, job.TargetPath);
        return isRewritten
            ? FileResult.Optimized(job.File, originalSize, optimizedSize)
            : FileResult.AlreadyOptimized(job.File, originalSize);
    }

    private static void ReplaceWithRetries(string tempPath, string targetPath)
    {
        for (var attempt = 1; ; attempt++)
        {
            try
            {
                Replace(tempPath, targetPath);
                return;
            }
            catch (IOException ex) when (attempt < CommitAttempts && IsTransientLock(ex))
            {
                Thread.Sleep(CommitRetryDelay);
            }
        }
    }

    private static void Replace(string tempPath, string targetPath)
    {
        if (!File.Exists(targetPath))
        {
            File.Move(tempPath, targetPath);
            return;
        }

        var backupPath = tempPath + BackupExtension;
        try
        {
            File.Replace(tempPath, targetPath, backupPath, ignoreMetadataErrors: true);
        }
        catch (IOException) when (!File.Exists(targetPath) && File.Exists(backupPath))
        {
            File.Move(backupPath, targetPath);
            throw;
        }

        FileHelper.TryDelete(backupPath);
    }

    private static string PrepareTempPath(string targetPath)
    {
        var directory = Path.GetDirectoryName(targetPath) ?? throw new IOException($"{targetPath} has no parent folder.");
        Directory.CreateDirectory(directory);
        return Path.Combine(directory, TempPrefix + Guid.NewGuid().ToString("N").Substring(0, TempSuffixLength) + TempExtension);
    }

    // oxipng doesn't flush its output, and NTFS may persist the rename before the data after a power loss.
    private static void FlushToDisk(string path)
    {
        using var stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
        stream.Flush(flushToDisk: true);
    }

    private static (long Length, DateTime LastWriteTimeUtc) Snapshot(string path)
    {
        var info = new FileInfo(path);
        return (info.Length, info.LastWriteTimeUtc);
    }

    private static bool IsReadOnly(string path) => File.Exists(path) && (File.GetAttributes(path) & FileAttributes.ReadOnly) != 0;

    private static bool IsTransientLock(IOException exception) =>
        (exception.HResult & Win32ErrorCodeMask) is ErrorSharingViolation or ErrorLockViolation or ErrorUnableToRemoveReplaced;
}
