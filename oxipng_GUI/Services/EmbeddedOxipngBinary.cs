using System.ComponentModel;

using OxipngGui.Utils;

namespace OxipngGui.Services;

/// <summary>
/// Extracts the oxipng.exe embedded in this app to a per-user folder and keeps it open read-only while the app runs,
/// so it can't be replaced or deleted between the content check and its use.
/// </summary>
public sealed class EmbeddedOxipngBinary : IOxipngBinary, IDisposable
{
    private const string ExecutableName = "oxipng.exe";
    private const string VersionArgument = "--version";
    private const string TempExtension = ".tmp";
    private const int SuccessExitCode = 0;
    private const int DllNotFoundExitCode = unchecked((int)0xC0000135);

    private readonly string _cacheDirectory;
    private readonly IProcessRunner _processRunner;
    private readonly object _gate = new();
    private FileStream? _executableLock;
    private string? _verifiedPath;

    public EmbeddedOxipngBinary(string cacheDirectory, IProcessRunner processRunner)
    {
        if (string.IsNullOrWhiteSpace(cacheDirectory))
            throw new ArgumentException("The cache directory must not be empty.", nameof(cacheDirectory));

        _cacheDirectory = cacheDirectory;
        _processRunner = processRunner ?? throw new ArgumentNullException(nameof(processRunner));
        Version = EmbeddedResources.Metadata(EmbeddedResources.OxipngVersionKey);
    }

    public string Version { get; }

    public string EnsureAvailable()
    {
        lock (_gate)
        {
            return _verifiedPath ??= ExtractAndCheck();
        }
    }

    public void Dispose()
    {
        lock (_gate)
        {
            _executableLock?.Dispose();
            _executableLock = null;
            _verifiedPath = null;
        }
    }

    private string ExtractAndCheck()
    {
        var path = Extract();
        CheckStarts(path);
        DeleteOtherVersions();
        return path;
    }

    private string Extract()
    {
        var directory = Path.Combine(_cacheDirectory, Version);
        var path = Path.Combine(directory, ExecutableName);
        var expected = EmbeddedResources.ReadBytes(EmbeddedResources.OxipngExecutable);
        try
        {
            if (!HasContent(path, expected))
                WriteAtomically(directory, path, expected);

            LockVerified(path, expected);
            return path;
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            throw new OxipngUnavailableException($"oxipng couldn't be extracted to {directory}: {ex.Message}", isRuntimeMissing: false, ex);
        }
    }

    private void LockVerified(string path, byte[] expected)
    {
        var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        if (!HasContent(stream, expected))
        {
            stream.Dispose();
            throw new IOException($"{path} doesn't match the oxipng.exe embedded in this app.");
        }

        _executableLock?.Dispose();
        _executableLock = stream;
    }

    private void CheckStarts(string path)
    {
        ProcessResult result;
        try
        {
            result = _processRunner.Run(path, [VersionArgument]);
        }
        catch (Win32Exception ex)
        {
            throw new OxipngUnavailableException($"oxipng couldn't be started: {ex.Message}", isRuntimeMissing: false, ex);
        }

        if (result.ExitCode == DllNotFoundExitCode)
            throw new OxipngUnavailableException("oxipng needs the Microsoft Visual C++ Redistributable (x64), which isn't installed on this PC.", isRuntimeMissing: true);
        if (result.ExitCode != SuccessExitCode)
            throw new OxipngUnavailableException($"oxipng couldn't be started (exit code {result.ExitCode}).", isRuntimeMissing: false);
    }

    private void DeleteOtherVersions()
    {
        foreach (var directory in Directory.EnumerateDirectories(_cacheDirectory))
        {
            if (!string.Equals(Path.GetFileName(directory), Version, StringComparison.OrdinalIgnoreCase))
                FileHelper.TryDeleteDirectory(directory);
        }
    }

    private static void WriteAtomically(string directory, string path, byte[] content)
    {
        Directory.CreateDirectory(directory);
        var tempPath = Path.Combine(directory, Guid.NewGuid().ToString("N") + TempExtension);
        try
        {
            File.WriteAllBytes(tempPath, content);
            if (File.Exists(path))
                File.Delete(path);
            File.Move(tempPath, path);
        }
        catch (IOException) when (HasContent(path, content))
        {
            // Another instance of the app extracted the same file at the same time.
        }
        finally
        {
            FileHelper.TryDelete(tempPath);
        }
    }

    private static bool HasContent(string path, byte[] expected)
    {
        if (!File.Exists(path))
            return false;

        using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
        return HasContent(stream, expected);
    }

    private static bool HasContent(Stream stream, byte[] expected)
    {
        if (stream.Length != expected.Length)
            return false;

        var actual = new byte[expected.Length];
        stream.Position = 0;
        var read = 0;
        while (read < actual.Length)
        {
            var count = stream.Read(actual, read, actual.Length - read);
            if (count == 0)
                return false;
            read += count;
        }

        return actual.SequenceEqual(expected);
    }
}
