using OxipngGui.Models;

namespace OxipngGui.Services;

public interface IPngFileCollector
{
    /// <summary>
    /// Turns dropped or picked paths into PNG files. Folders are searched recursively; paths that aren't PNG files
    /// and folders that can't be read are counted as skipped.
    /// </summary>
    /// <param name="progress">Gets the number of PNG files found so far, after each file.</param>
    /// <exception cref="OperationCanceledException">The search was stopped.</exception>
    CollectedFiles Collect(IEnumerable<string> paths, IProgress<int>? progress = null, CancellationToken cancellationToken = default);
}
