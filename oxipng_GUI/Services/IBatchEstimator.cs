using OxipngGui.Models;

namespace OxipngGui.Services;

/// <summary>Estimates the progress of the batch whose runs it observes.</summary>
public interface IBatchEstimator : IRunObserver
{
    /// <summary>Safe to call from any thread while the batch runs.</summary>
    /// <returns>The estimate, or <see langword="null"/> while too few runs have finished to make one.</returns>
    BatchEstimate? Estimate();
}
