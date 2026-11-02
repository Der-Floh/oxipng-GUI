namespace OxipngGui.Services;

/// <summary>
/// Names the temp files and folders that oxipng's results go to before they replace their targets, and checks that
/// they stay within the path lengths Windows accepts.
/// </summary>
public static class TempPaths
{
    private const string Prefix = "~oxipng-";
    private const int SuffixLength = 8;
    private const string FileExtension = ".tmp";
    private const string BackupExtension = ".bak";
    private static readonly int NameLength = Prefix.Length + SuffixLength;
    private static readonly int FileNameLength = NameLength + Math.Max(FileExtension.Length, BackupExtension.Length);

    // Without long-path support Windows refuses longer paths, while oxipng would still write them.
    private const int MaxPathLength = 259;
    private const int MaxFolderLength = 247;

    public static string NewFileIn(string folder) => Path.Combine(folder, NewName() + FileExtension);

    public static string NewBackupIn(string folder) => Path.Combine(folder, NewName() + BackupExtension);

    public static string NewFolderIn(string folder) => Path.Combine(folder, NewName());

    /// <summary>Whether <paramref name="folder"/> has room for a temp file or a backup.</summary>
    public static bool FitsFile(string folder) => folder.Length + 1 + FileNameLength <= MaxPathLength;

    /// <summary>Whether a temp folder in <paramref name="folder"/> has room for the result of <paramref name="inputPath"/>.</summary>
    public static bool FitsResult(string folder, string inputPath)
    {
        var tempFolderLength = folder.Length + 1 + NameLength;
        return tempFolderLength <= MaxFolderLength && tempFolderLength + 1 + Path.GetFileName(inputPath).Length <= MaxPathLength;
    }

    private static string NewName() => Prefix + Guid.NewGuid().ToString("N").Substring(0, SuffixLength);
}
