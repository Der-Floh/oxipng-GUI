using OxipngGui.Models;

namespace OxipngGui.Services;

public interface IPngFileCollector
{
    /// <summary>
    /// Turns dropped or picked paths into PNG files. Folders are searched recursively; paths that aren't PNG files
    /// and folders that can't be read are counted as skipped.
    /// </summary>
    CollectedFiles Collect(IEnumerable<string> paths);
}
