using OxipngGui.Models;

namespace OxipngGui.Services;

public interface IPngOptimizer
{
    /// <summary>
    /// Optimizes a group of files with one oxipng run on the calling thread and returns a result for each job, in the
    /// group's order. Problems with a file come back as <see cref="FileStatus.Failed"/> and cancelling as
    /// <see cref="FileStatus.Stopped"/>; the original stays untouched in both cases.
    /// </summary>
    IReadOnlyList<FileResult> Optimize(JobGroup group, OptimizationOptions options, int threads, CancellationToken cancellationToken = default);
}
