using OxipngGui.Models;

namespace OxipngGui.Services;

public static class OutputPlanner
{
    /// <summary>
    /// Works out where each file is written. A file whose target is already taken by another file in the batch,
    /// or is another file's original, becomes a conflict instead of a job, so no two jobs write the same file.
    /// </summary>
    public static JobPlan Plan(IReadOnlyList<PngFile> files, OutputOptions output)
    {
        if (files is null)
            throw new ArgumentNullException(nameof(files));
        if (output is null)
            throw new ArgumentNullException(nameof(output));

        var sources = new HashSet<string>(files.Select(file => file.FullPath), StringComparer.OrdinalIgnoreCase);
        var claimedTargets = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        List<PngJob> jobs = [];
        List<PngFile> conflicts = [];
        foreach (var file in files)
        {
            var job = new PngJob(file, TargetPathFor(file, output));
            var overwritesOtherOriginal = !job.OverwritesOriginal && sources.Contains(job.TargetPath);
            if (!overwritesOtherOriginal && claimedTargets.Add(job.TargetPath))
                jobs.Add(job);
            else
                conflicts.Add(file);
        }

        return new JobPlan(jobs, conflicts);
    }

    private static string TargetPathFor(PngFile file, OutputOptions output) => output.Folder is null
        ? file.FullPath
        : Path.GetFullPath(Path.Combine(output.Folder, file.RelativeOutputPath));
}
