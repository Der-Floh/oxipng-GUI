using OxipngGui.Models;

namespace OxipngGui.Services;

public interface IBatchOptimizer
{
    /// <summary>
    /// Optimizes the files in parallel and reports every status change through <paramref name="progress"/>.
    /// </summary>
    /// <param name="observer">Follows the oxipng runs, for example to estimate how long the rest takes.</param>
    /// <exception cref="OxipngUnavailableException">oxipng can't run on this PC; no file was touched.</exception>
    /// <exception cref="OperationCanceledException">The batch was stopped; unfinished files are left as they were.</exception>
    Task RunAsync(IReadOnlyList<PngFile> files, OptimizationOptions options, OutputOptions output, IProgress<FileResult> progress, IRunObserver? observer = null, CancellationToken cancellationToken = default);
}
