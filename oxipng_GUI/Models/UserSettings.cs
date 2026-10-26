namespace OxipngGui.Models;

public sealed class UserSettings(OptimizationOptions options, bool saveToFolder, string outputFolder, bool showAdvanced, string lastBrowseFolder)
{
    public OptimizationOptions Options { get; } = options;

    public bool SaveToFolder { get; } = saveToFolder;

    public string OutputFolder { get; } = outputFolder;

    public bool ShowAdvanced { get; } = showAdvanced;

    public string LastBrowseFolder { get; } = lastBrowseFolder;

    public OutputOptions Output => SaveToFolder && OutputFolder.Length > 0 ? OutputOptions.ToFolder(OutputFolder) : OutputOptions.Overwrite;

    public override string ToString() => $"{Options}; output: {Output}";
}
