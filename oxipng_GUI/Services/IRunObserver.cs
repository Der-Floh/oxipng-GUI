using OxipngGui.Models;

namespace OxipngGui.Services;

/// <summary>Follows the oxipng runs of a batch. Runs start and finish on worker threads.</summary>
public interface IRunObserver
{
    /// <summary>Called once, before the first run starts, with every run of the batch.</summary>
    /// <param name="parallelRuns">How many runs go at a time.</param>
    void RunsPlanned(IReadOnlyList<JobGroup> runs, int parallelRuns);

    void RunStarted(JobGroup run);

    /// <summary>Called when a run is done; a run that was stopped doesn't finish.</summary>
    void RunFinished(JobGroup run);
}
