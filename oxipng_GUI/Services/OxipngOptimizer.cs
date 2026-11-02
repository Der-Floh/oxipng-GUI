using System.ComponentModel;

using OxipngGui.Models;
using OxipngGui.Utils;

namespace OxipngGui.Services;

/// <summary>
/// Optimizes a group of files with one oxipng run. oxipng truncates and rewrites files in place, so it always writes
/// to a temp file next to the target, or for several files to a temp folder next to the group's first target, and
/// each result replaces its target only once it's complete. Stopping oxipng never damages an original.
/// </summary>
public sealed class OxipngOptimizer(IOxipngBinary oxipng, IProcessRunner processRunner) : IPngOptimizer
{
    private const int CommitAttempts = 5;
    private const int Win32ErrorCodeMask = 0xFFFF;
    private const int ErrorSharingViolation = 32;
    private const int ErrorLockViolation = 33;
    private const int ErrorUnableToRemoveReplaced = 1175;
    private const string ReadOnlyMessage = "The file is read-only.";
    private const string PathTooLongMessage = "The path is too long. Move the file to a folder with a shorter path.";
    private const string ChangedMessage = "The file changed while it was being optimized.";
    private const string UnknownErrorMessage = "oxipng reported an unknown error.";
    private const string MissingResultMessage = "oxipng reported no result for this file.";
    private static readonly TimeSpan CommitRetryDelay = TimeSpan.FromMilliseconds(200);

    private readonly IOxipngBinary _oxipng = oxipng ?? throw new ArgumentNullException(nameof(oxipng));
    private readonly IProcessRunner _processRunner = processRunner ?? throw new ArgumentNullException(nameof(processRunner));

    public IReadOnlyList<FileResult> Optimize(JobGroup group, OptimizationOptions options, int threads, CancellationToken cancellationToken = default)
    {
        if (group is null)
            throw new ArgumentNullException(nameof(group));
        if (options is null)
            throw new ArgumentNullException(nameof(options));

        var results = new Dictionary<PngJob, FileResult>();
        var runnable = new Dictionary<PngJob, (long Length, DateTime LastWriteTimeUtc)>();
        foreach (var job in group.Jobs)
        {
            try
            {
                var before = Snapshot(job.File.FullPath);
                if (IsReadOnly(job.TargetPath))
                    results[job] = FileResult.Failed(job.File, ReadOnlyMessage);
                else if (!TempPaths.FitsFile(FolderOf(job.TargetPath)))
                    results[job] = FileResult.Failed(job.File, PathTooLongMessage);
                else
                    runnable[job] = before;
            }
            catch (Exception ex) when (IsFileProblem(ex))
            {
                results[job] = FileResult.Failed(job.File, ex.Message);
            }
        }

        if (runnable.Count > 0)
        {
            foreach (var result in OptimizeRunnable(FolderOf(group.Jobs[0].TargetPath), runnable, options, threads, cancellationToken))
                results[result.Key] = result.Value;
        }

        return [.. group.Jobs.Select(job => results[job])];
    }

    /// <param name="hostFolder">Where the temp folder for several files goes. <see cref="JobGrouper"/> makes sure each result fits there.</param>
    private Dictionary<PngJob, FileResult> OptimizeRunnable(string hostFolder, Dictionary<PngJob, (long Length, DateTime LastWriteTimeUtc)> runnable, OptimizationOptions options, int threads, CancellationToken cancellationToken)
    {
        var jobs = runnable.Keys.ToList();
        var tempFolder = jobs.Count > 1 ? TempPaths.NewFolderIn(hostFolder) : null;
        var outputs = new Dictionary<PngJob, string>();
        try
        {
            foreach (var job in jobs)
                outputs[job] = tempFolder is null ? TempPaths.NewFileIn(FolderOf(job.TargetPath)) : Path.Combine(tempFolder, Path.GetFileName(job.File.FullPath));

            Directory.CreateDirectory(tempFolder ?? FolderOf(jobs[0].TargetPath));
            var arguments = tempFolder is null
                ? OxipngArguments.ForFile(options, threads, jobs[0].File.FullPath, outputs[jobs[0]])
                : OxipngArguments.ForFiles(options, threads, [.. jobs.Select(job => job.File.FullPath)], tempFolder);
            var reports = RunOxipng(arguments, cancellationToken);
            return jobs.ToDictionary(job => job, job => Complete(job, runnable[job], reports, outputs[job], options.ForceWrite));
        }
        catch (OperationCanceledException)
        {
            return jobs.ToDictionary(job => job, job => FileResult.Stopped(job.File));
        }
        catch (Exception ex) when (IsFileProblem(ex) || ex is Win32Exception)
        {
            return jobs.ToDictionary(job => job, job => FileResult.Failed(job.File, ex.Message));
        }
        finally
        {
            CleanUp(outputs, tempFolder);
        }
    }

    private Dictionary<string, OxipngReport> RunOxipng(IReadOnlyList<string> arguments, CancellationToken cancellationToken)
    {
        var result = _processRunner.Run(_oxipng.EnsureAvailable(), arguments, cancellationToken);
        var reports = OxipngReport.Parse(result.StandardOutput)
            ?? throw new IOException($"oxipng stopped without a result (exit code {result.ExitCode}).");

        var byInput = new Dictionary<string, OxipngReport>(StringComparer.OrdinalIgnoreCase);
        foreach (var report in reports)
            byInput[report.Input] = report;

        return byInput;
    }

    private static FileResult Complete(PngJob job, (long Length, DateTime LastWriteTimeUtc) before, Dictionary<string, OxipngReport> reports, string outputPath, bool forceWrite)
    {
        if (!reports.TryGetValue(job.File.FullPath, out var report))
            return FileResult.Failed(job.File, MissingResultMessage);
        if (!report.Succeeded)
            return FileResult.Failed(job.File, report.Error ?? UnknownErrorMessage);

        try
        {
            return Snapshot(job.File.FullPath) == before
                ? Commit(job, outputPath, before.Length, forceWrite)
                : FileResult.Failed(job.File, ChangedMessage);
        }
        catch (Exception ex) when (IsFileProblem(ex))
        {
            return FileResult.Failed(job.File, ex.Message);
        }
    }

    private static FileResult Commit(PngJob job, string outputPath, long originalSize, bool forceWrite)
    {
        var optimizedSize = new FileInfo(outputPath).Length;
        var isRewritten = optimizedSize < originalSize || forceWrite;
        if (job.OverwritesOriginal && !isRewritten)
            return FileResult.AlreadyOptimized(job.File, originalSize);

        FlushToDisk(outputPath);
        Directory.CreateDirectory(FolderOf(job.TargetPath));
        ReplaceWithRetries(outputPath, job.TargetPath);
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

        // Next to its target, a backup that a power loss left behind is easy to find and to rename back.
        var backupPath = TempPaths.NewBackupIn(FolderOf(targetPath));
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

    // An output whose original is gone holds the only copy of that image, so it stays.
    private static void CleanUp(Dictionary<PngJob, string> outputs, string? tempFolder)
    {
        foreach (var output in outputs.Where(output => File.Exists(output.Key.File.FullPath)))
            FileHelper.TryDelete(output.Value);

        if (tempFolder is not null)
            FileHelper.TryDeleteEmptyDirectory(tempFolder);
    }

    private static string FolderOf(string path) => Path.GetDirectoryName(path) ?? throw new IOException($"{path} has no parent folder.");

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

    private static bool IsFileProblem(Exception exception) => exception is IOException or UnauthorizedAccessException;

    private static bool IsTransientLock(IOException exception) =>
        (exception.HResult & Win32ErrorCodeMask) is ErrorSharingViolation or ErrorLockViolation or ErrorUnableToRemoveReplaced;
}
