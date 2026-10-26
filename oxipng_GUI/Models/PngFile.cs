namespace OxipngGui.Models;

/// <summary>A PNG file in the list.</summary>
/// <param name="relativeOutputPath">
/// Where the file goes below an output folder: its path relative to the parent of the folder it was added with,
/// or just its name when it was added on its own.
/// </param>
public sealed class PngFile(string fullPath, string relativeOutputPath, long size)
{
    public string FullPath { get; } = fullPath;

    public string RelativeOutputPath { get; } = relativeOutputPath;

    public long Size { get; } = size;

    public override string ToString() => FullPath;
}
