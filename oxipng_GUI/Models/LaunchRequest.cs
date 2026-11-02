namespace OxipngGui.Models;

/// <summary>What the window does right after it opens.</summary>
public sealed class LaunchRequest(IReadOnlyList<string> paths, bool optimizeOnStart)
{
    public static LaunchRequest None { get; } = new([], false);

    public IReadOnlyList<string> Paths { get; } = paths;

    public bool OptimizeOnStart { get; } = optimizeOnStart;

    public override string ToString() => $"{Paths.Count} paths, optimize on start: {OptimizeOnStart}";
}
