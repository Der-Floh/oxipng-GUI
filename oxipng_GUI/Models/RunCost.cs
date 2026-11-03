namespace OxipngGui.Models;

/// <summary>How long an oxipng run takes: a cost for the run itself, plus a cost per file and per megabyte.</summary>
public sealed class RunCost(double secondsPerRun, double secondsPerFile, double secondsPerMegabyte)
{
    private const double BytesPerMegabyte = 1024 * 1024;

    public double SecondsPerRun { get; } = secondsPerRun;

    public double SecondsPerFile { get; } = secondsPerFile;

    public double SecondsPerMegabyte { get; } = secondsPerMegabyte;

    /// <summary>How long <paramref name="runs"/> runs with <paramref name="files"/> files and <paramref name="bytes"/> bytes in total take, one after another.</summary>
    public double Seconds(int runs, int files, long bytes) =>
        SecondsPerRun * runs + SecondsPerFile * files + SecondsPerMegabyte * bytes / BytesPerMegabyte;

    public override string ToString() => $"{SecondsPerRun:0.000} s per run + {SecondsPerFile:0.000} s per file + {SecondsPerMegabyte:0.000} s per MB";
}
