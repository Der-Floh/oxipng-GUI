namespace OxipngGui.Services;

public sealed class ProcessResult(int exitCode, string standardOutput)
{
    public int ExitCode { get; } = exitCode;

    public string StandardOutput { get; } = standardOutput;

    public override string ToString() => $"Exit code {ExitCode}";
}
