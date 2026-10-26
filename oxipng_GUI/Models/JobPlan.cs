namespace OxipngGui.Models;

public sealed class JobPlan(IReadOnlyList<PngJob> jobs, IReadOnlyList<PngFile> conflicts)
{
    public IReadOnlyList<PngJob> Jobs { get; } = jobs;

    public IReadOnlyList<PngFile> Conflicts { get; } = conflicts;

    public override string ToString() => $"{Jobs.Count} jobs, {Conflicts.Count} conflicts";
}
