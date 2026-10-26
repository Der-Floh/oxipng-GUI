using System.Security;

using OxipngGui.Models;
using OxipngGui.Utils;

namespace OxipngGui.Services;

public sealed class PngFileCollector : IPngFileCollector
{
    private static readonly string[] PngExtensions = [".png", ".apng"];
    private static readonly char[] PathSeparators = [Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar];

    public CollectedFiles Collect(IEnumerable<string> paths)
    {
        if (paths is null)
            throw new ArgumentNullException(nameof(paths));

        List<PngFile> files = [];
        var skipped = 0;
        foreach (var path in paths)
        {
            var fullPath = FullPathOrNull(path);
            if (fullPath is not null && Directory.Exists(fullPath))
                skipped += CollectFolder(fullPath, files);
            else if (fullPath is not null && IsPng(fullPath) && File.Exists(fullPath))
                files.Add(new PngFile(fullPath, Path.GetFileName(fullPath), new FileInfo(fullPath).Length));
            else
                skipped++;
        }

        return new CollectedFiles(files, skipped);
    }

    /// <summary>
    /// Adds every PNG below <paramref name="folder"/>, without following junctions or symbolic links.
    /// </summary>
    /// <returns>The number of subfolders that couldn't be read.</returns>
    private static int CollectFolder(string folder, List<PngFile> files)
    {
        var relativeBase = RelativeBaseOf(folder);
        var unreadable = 0;
        List<PngFile> found = [];
        var pending = new Stack<DirectoryInfo>();
        pending.Push(new DirectoryInfo(folder));
        while (pending.Count > 0)
        {
            var directory = pending.Pop();
            try
            {
                foreach (var entry in directory.EnumerateFileSystemInfos())
                {
                    if (entry is DirectoryInfo subdirectory && (subdirectory.Attributes & FileAttributes.ReparsePoint) == 0)
                        pending.Push(subdirectory);
                    else if (entry is FileInfo file && IsPng(file.Name))
                        found.Add(new PngFile(file.FullName, file.FullName.Substring(relativeBase.Length), file.Length));
                }
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or SecurityException)
            {
                unreadable++;
            }
        }

        found.Sort((a, b) => NaturalFileNameComparer.Instance.Compare(a.FullPath, b.FullPath));
        files.AddRange(found);
        return unreadable;
    }

    /// <summary>
    /// The parent of <paramref name="folder"/>, so output paths keep the folder's own name, like an Explorer copy.
    /// A drive root has no parent and is its own base.
    /// </summary>
    private static string RelativeBaseOf(string folder)
    {
        var parent = Path.GetDirectoryName(folder.TrimEnd(PathSeparators)) ?? folder;
        return parent.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal)
            ? parent
            : parent + Path.DirectorySeparatorChar;
    }

    private static bool IsPng(string path) => PngExtensions.Contains(Path.GetExtension(path), StringComparer.OrdinalIgnoreCase);

    private static string? FullPathOrNull(string path)
    {
        try
        {
            return Path.GetFullPath(path);
        }
        catch (Exception ex) when (ex is ArgumentException or NotSupportedException or IOException or SecurityException)
        {
            return null;
        }
    }
}
