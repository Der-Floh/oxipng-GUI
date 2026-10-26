using OxipngGui.Models;

namespace OxipngGui.Services;

public interface IPngOptimizer
{
    /// <summary>
    /// Optimizes one file on the calling thread. Problems with the file come back as <see cref="FileStatus.Failed"/>
    /// and cancelling as <see cref="FileStatus.Stopped"/>; the original stays untouched in both cases.
    /// </summary>
    FileResult Optimize(PngJob job, OptimizationOptions options, int threads, CancellationToken cancellationToken = default);
}
