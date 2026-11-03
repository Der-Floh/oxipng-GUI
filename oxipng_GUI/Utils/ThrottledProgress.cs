using System.Diagnostics;

namespace OxipngGui.Utils;

/// <summary>
/// Passes on at most one report per interval, starting one interval in, so a loop that reports every step neither
/// floods the UI thread nor shows progress for work that's over in a moment. Reports must come from one thread at a
/// time.
/// </summary>
public sealed class ThrottledProgress<T>(IProgress<T> target, TimeSpan interval) : IProgress<T>
{
    private readonly IProgress<T> _target = target ?? throw new ArgumentNullException(nameof(target));
    private readonly TimeSpan _interval = interval;
    private readonly Stopwatch _watch = Stopwatch.StartNew();
    private TimeSpan _nextReport = interval;

    public void Report(T value)
    {
        var now = _watch.Elapsed;
        if (now < _nextReport)
            return;

        _nextReport = now + _interval;
        _target.Report(value);
    }
}
