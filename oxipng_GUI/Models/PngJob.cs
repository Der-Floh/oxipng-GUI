namespace OxipngGui.Models;

public sealed class PngJob(PngFile file, string targetPath)
{
    public PngFile File { get; } = file;

    public string TargetPath { get; } = targetPath;

    public bool OverwritesOriginal => string.Equals(TargetPath, File.FullPath, StringComparison.OrdinalIgnoreCase);

    public override string ToString() => $"{File.FullPath} -> {TargetPath}";
}
