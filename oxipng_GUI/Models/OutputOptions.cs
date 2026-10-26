namespace OxipngGui.Models;

public sealed class OutputOptions
{
    private OutputOptions(string? folder)
    {
        Folder = folder;
    }

    public static OutputOptions Overwrite { get; } = new(null);

    public string? Folder { get; }

    public static OutputOptions ToFolder(string folder) => string.IsNullOrWhiteSpace(folder)
        ? throw new ArgumentException("The output folder must not be empty.", nameof(folder))
        : new(Path.GetFullPath(folder));

    public override string ToString() => Folder ?? "Overwrite originals";
}
