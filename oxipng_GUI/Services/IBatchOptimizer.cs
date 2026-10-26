using OxipngGui.Models;

namespace OxipngGui.Services;

public interface IBatchOptimizer
{
    /// <summary>
    /// Optimizes the files in parallel and reports every status change through <paramref name="progress"/>.
    /// </summary>
    /// <exception cref="OxipngUnavailableException">oxipng can't run on this PC; no file was touched.</exception>
    /// <exception cref="OperationCanceledException">The batch was stopped; unfinished files are left as they were.</exception>
    Task RunAsync(IReadOnlyList<PngFile> files, OptimizationOptions options, OutputOptions output, IProgress<FileResult> progress, CancellationToken cancellationToken = default);
}
