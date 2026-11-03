using System.Diagnostics;

using OxipngGui.Models;

namespace OxipngGui.Services;

/// <summary>
/// Times each oxipng run of a batch and learns from the finished ones how long a run takes for its files and bytes.
/// That gives every run a weight, so large files that take long count for more than small ones. The time left comes
/// from how fast the weighted work has gone so far, which also covers how well the runs share the processors.
/// </summary>
public sealed class BatchEstimator : IBatchEstimator
{
    private const int MinFinishedRuns = 3;
    private const double OverdueShare = 0.5;
    private const double SmoothingSeconds = 3;
    private static readonly TimeSpan MinElapsed = TimeSpan.FromSeconds(5);

    private readonly object _lock = new();
    private readonly Func<TimeSpan> _clock;
    private readonly RunCostModel _costModel = new();
    private readonly Dictionary<JobGroup, TimeSpan> _running = [];
    private TimeSpan _plannedAt;
    private int _parallelRuns = 1;
    private int _queuedRuns;
    private int _queuedFiles;
    private long _queuedBytes;
    private int _finishedRuns;
    private int _finishedFiles;
    private long _finishedBytes;
    private TimeSpan? _lastEstimateAt;
    private double _lastRemaining;

    public BatchEstimator()
        : this(StartClock())
    {
    }

    /// <param name="clock">The time since some fixed point; runs are timed with it.</param>
    public BatchEstimator(Func<TimeSpan> clock)
    {
        _clock = clock ?? throw new ArgumentNullException(nameof(clock));
    }

    public void RunsPlanned(IReadOnlyList<JobGroup> runs, int parallelRuns)
    {
        if (runs is null)
            throw new ArgumentNullException(nameof(runs));
        if (parallelRuns < 1)
            throw new ArgumentOutOfRangeException(nameof(parallelRuns), parallelRuns, "At least one run must go at a time.");

        lock (_lock)
        {
            _plannedAt = _clock();
            _parallelRuns = parallelRuns;
            _queuedRuns = runs.Count;
            _queuedFiles = runs.Sum(run => run.Jobs.Count);
            _queuedBytes = runs.Sum(run => run.Size);
        }
    }

    public void RunStarted(JobGroup run)
    {
        if (run is null)
            throw new ArgumentNullException(nameof(run));

        lock (_lock)
        {
            _queuedRuns--;
            _queuedFiles -= run.Jobs.Count;
            _queuedBytes -= run.Size;
            _running[run] = _clock();
        }
    }

    public void RunFinished(JobGroup run)
    {
        if (run is null)
            throw new ArgumentNullException(nameof(run));

        lock (_lock)
        {
            if (!_running.TryGetValue(run, out var startedAt))
                throw new InvalidOperationException("A run finished that never started.");

            var duration = _clock() - startedAt;
            _running.Remove(run);
            _costModel.Add(run.Jobs.Count, run.Size, duration);
            _finishedRuns++;
            _finishedFiles += run.Jobs.Count;
            _finishedBytes += run.Size;
        }
    }

    /// <summary>
    /// Waits until as many runs finished as go at a time: the first runs all start together, so the ones that finish
    /// first are the quick ones and would make every run look quick.
    /// </summary>
    public BatchEstimate? Estimate()
    {
        lock (_lock)
        {
            var elapsed = _clock() - _plannedAt;
            if (_finishedRuns < Math.Max(MinFinishedRuns, _parallelRuns) || elapsed < MinElapsed)
                return null;

            var cost = _costModel.Fit();
            return cost is null ? null : Estimate(cost, elapsed);
        }
    }

    /// <summary>
    /// Counts a running run as done for the time it ran so far, up to its prediction; one that runs longer than
    /// predicted is expected to need half as long again as it has run. The time left is how long the rest takes at the
    /// pace so far, but at least until the running run with the most left is predicted to finish.
    /// </summary>
    private BatchEstimate? Estimate(RunCost cost, TimeSpan elapsed)
    {
        var now = _plannedAt + elapsed;
        var done = cost.Seconds(_finishedRuns, _finishedFiles, _finishedBytes);
        var left = cost.Seconds(_queuedRuns, _queuedFiles, _queuedBytes);
        var longestLeft = 0.0;
        foreach (var running in _running)
        {
            var predicted = cost.Seconds(1, running.Key.Jobs.Count, running.Key.Size);
            var ran = (now - running.Value).TotalSeconds;
            var runLeft = Math.Max(predicted - ran, OverdueShare * ran);
            done += Math.Min(ran, predicted);
            left += runLeft;
            longestLeft = Math.Max(longestLeft, runLeft);
        }

        if (done <= 0)
            return null;

        var remaining = Smooth(Math.Max(elapsed.TotalSeconds * left / done, longestLeft), elapsed);
        return new BatchEstimate(done / (done + left), TimeSpan.FromSeconds(remaining));
    }

    /// <summary>
    /// Counts the last estimate down by the time since, then moves it part of the way to the new one, so the time left
    /// doesn't jump back and forth between two values.
    /// </summary>
    private double Smooth(double remaining, TimeSpan elapsed)
    {
        if (_lastEstimateAt is TimeSpan lastEstimateAt)
        {
            var step = (elapsed - lastEstimateAt).TotalSeconds;
            var counted = Math.Max(0, _lastRemaining - step);
            remaining = counted + (remaining - counted) * (1 - Math.Exp(-step / SmoothingSeconds));
        }

        _lastEstimateAt = elapsed;
        _lastRemaining = remaining;
        return remaining;
    }

    private static Func<TimeSpan> StartClock()
    {
        var watch = Stopwatch.StartNew();
        return () => watch.Elapsed;
    }
}
