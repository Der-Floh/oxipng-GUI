using System.ComponentModel;
using System.Diagnostics;
using System.Text;

using OxipngGui.Utils;

namespace OxipngGui.Services;

/// <summary>
/// Runs a process synchronously on the calling thread. On .NET Framework, pipe reads block a thread either way,
/// so callers run it on dedicated threads instead of tying up the thread pool.
/// </summary>
public sealed class ProcessRunner(KillOnCloseJob job) : IProcessRunner
{
    private static readonly Encoding Utf8 = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);

    private readonly KillOnCloseJob _job = job ?? throw new ArgumentNullException(nameof(job));

    public ProcessResult Run(string executablePath, IReadOnlyList<string> arguments, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(executablePath))
            throw new ArgumentException("The executable path must not be empty.", nameof(executablePath));
        if (arguments is null)
            throw new ArgumentNullException(nameof(arguments));

        cancellationToken.ThrowIfCancellationRequested();
        using var process = new Process { StartInfo = CreateStartInfo(executablePath, CommandLine.Join(arguments)) };
        process.Start();
        _job.TryAssign(process);

        string standardOutput;
        using (cancellationToken.Register(() => TryKill(process)))
        {
            standardOutput = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
        }

        cancellationToken.ThrowIfCancellationRequested();
        return new ProcessResult(process.ExitCode, standardOutput);
    }

    private static ProcessStartInfo CreateStartInfo(string executablePath, string arguments) => new(executablePath, arguments)
    {
        UseShellExecute = false,
        CreateNoWindow = true,
        RedirectStandardOutput = true,
        StandardOutputEncoding = Utf8,
    };

    private static void TryKill(Process process)
    {
        try
        {
            process.Kill();
        }
        catch (InvalidOperationException) { }
        catch (Win32Exception) { }
    }
}
