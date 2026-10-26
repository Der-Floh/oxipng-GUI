namespace OxipngGui.Models;

public sealed class CollectedFiles(IReadOnlyList<PngFile> files, int skippedCount)
{
    public IReadOnlyList<PngFile> Files { get; } = files;

    public int SkippedCount { get; } = skippedCount;

    public override string ToString() => $"{Files.Count} files, {SkippedCount} skipped";
}
