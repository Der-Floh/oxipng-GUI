namespace OxipngGui.Models;

/// <summary>How far a batch has got and how long the rest will take.</summary>
/// <param name="completedFraction">
/// The share of the batch's work that's done, from 0 to 1, with each file weighted by how long it takes.
/// </param>
public sealed class BatchEstimate(double completedFraction, TimeSpan remaining)
{
    public double CompletedFraction { get; } = completedFraction;

    public TimeSpan Remaining { get; } = remaining;

    public override string ToString() => $"{CompletedFraction:P0} done, {Remaining:g} left";
}
