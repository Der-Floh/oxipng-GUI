namespace OxipngGui.Services;

public interface IProcessRunner
{
    /// <summary>
    /// Runs a console program without a window and returns its exit code and standard output. Cancelling kills the
    /// process and throws <see cref="OperationCanceledException"/>.
    /// </summary>
    ProcessResult Run(string executablePath, IReadOnlyList<string> arguments, CancellationToken cancellationToken = default);
}
