namespace OxipngGui.Models;

/// <summary>
/// Jobs that one oxipng run handles together. With several jobs, the run's temp folder goes next to the first job's
/// target.
/// </summary>
public sealed class JobGroup(IReadOnlyList<PngJob> jobs)
{
    public IReadOnlyList<PngJob> Jobs { get; } = jobs;

    public long Size { get; } = jobs.Sum(job => job.File.Size);

    public override string ToString() => $"{Jobs.Count} jobs, {Size} bytes";
}
