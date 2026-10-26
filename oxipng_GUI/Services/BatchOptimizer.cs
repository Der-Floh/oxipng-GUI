using System.Collections.Concurrent;

using OxipngGui.Models;

namespace OxipngGui.Services;

/// <summary>
/// Runs one oxipng process per file, as many at a time as there are logical CPUs, and splits the CPUs between them
/// so a single file still gets every core. Zopfli spends most of its time on one core per file, so running files
/// side by side is what keeps a batch fast.
/// </summary>
public sealed class BatchOptimizer(IOxipngBinary oxipng, IPngOptimizer optimizer) : IBatchOptimizer
{
    private const string ConflictMessage = "Another file in the list would be written to the same place.";

    private readonly IOxipngBinary _oxipng = oxipng ?? throw new ArgumentNullException(nameof(oxipng));
    private readonly IPngOptimizer _optimizer = optimizer ?? throw new ArgumentNullException(nameof(optimizer));

    public async Task RunAsync(IReadOnlyList<PngFile> files, OptimizationOptions options, OutputOptions output, IProgress<FileResult> progress, CancellationToken cancellationToken = default)
    {
        if (options is null)
            throw new ArgumentNullException(nameof(options));
        if (progress is null)
            throw new ArgumentNullException(nameof(progress));

        var plan = OutputPlanner.Plan(files, output);
        foreach (var conflict in plan.Conflicts)
            progress.Report(FileResult.Failed(conflict, ConflictMessage));

        if (plan.Jobs.Count == 0)
            return;

        await Task.Run(() => _oxipng.EnsureAvailable(), cancellationToken).ConfigureAwait(false);

        var queue = new ConcurrentQueue<PngJob>(plan.Jobs.OrderByDescending(job => job.File.Size));
        var workerCount = Math.Min(plan.Jobs.Count, Environment.ProcessorCount);
        var threadsPerProcess = Math.Max(1, Environment.ProcessorCount / workerCount);
        var workers = Enumerable.Range(0, workerCount).Select(_ => Task.Factory.StartNew(
            () => Drain(queue, options, threadsPerProcess, progress, cancellationToken),
            CancellationToken.None,
            TaskCreationOptions.LongRunning,
            TaskScheduler.Default));

        await Task.WhenAll(workers).ConfigureAwait(false);
        cancellationToken.ThrowIfCancellationRequested();
    }

    private void Drain(ConcurrentQueue<PngJob> queue, OptimizationOptions options, int threads, IProgress<FileResult> progress, CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested && queue.TryDequeue(out var job))
        {
            progress.Report(FileResult.Optimizing(job.File));
            progress.Report(_optimizer.Optimize(job, options, threads, cancellationToken));
        }
    }
}
