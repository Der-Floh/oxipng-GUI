using OxipngGui.Models;

namespace OxipngGui.Services;

/// <summary>
/// Lets one oxipng run handle several small files. Starting oxipng takes about as long as optimizing a small PNG, so
/// a folder of many small files would otherwise spend most of its time starting processes.
/// </summary>
public static class JobGrouper
{
    private const long SmallFileLimit = 256 * 1024;
    private const long MaxGroupSize = 1024 * 1024;
    private const int MaxGroupFiles = 32;

    /// <summary>
    /// Returns the jobs in groups, largest group first. Large files run on their own. Small files share a group when
    /// their targets are on the same drive and their names differ, because oxipng writes a group's results into one
    /// temp folder, next to the group's first target, under the files' own names. Groups stay small enough to keep
    /// every processor busy.
    /// </summary>
    public static IReadOnlyList<JobGroup> Group(IReadOnlyList<PngJob> jobs, int processorCount)
    {
        if (jobs is null)
            throw new ArgumentNullException(nameof(jobs));
        if (processorCount < 1)
            throw new ArgumentOutOfRangeException(nameof(processorCount), processorCount, "At least one processor is needed.");

        var largestFirst = jobs.OrderByDescending(job => job.File.Size).ToList();
        var smallJobs = largestFirst.Where(IsSmall).ToList();
        var filesPerGroup = Math.Min(MaxGroupFiles, (smallJobs.Count + processorCount - 1) / processorCount);
        List<JobGroup> groups = [.. largestFirst.Where(job => !IsSmall(job)).Select(job => new JobGroup([job]))];
        groups.AddRange(GroupSmallJobs(smallJobs, Math.Max(1, filesPerGroup)));
        return [.. groups.OrderByDescending(group => group.Size)];
    }

    private static IEnumerable<JobGroup> GroupSmallJobs(IEnumerable<PngJob> jobs, int maxFiles)
    {
        List<OpenGroup> open = [];
        foreach (var job in jobs)
        {
            var group = open.Find(candidate => candidate.Accepts(job, maxFiles));
            if (group is not null)
            {
                group.Add(job);
            }
            else if (maxFiles > 1 && OpenGroup.CanHost(job))
            {
                group = new OpenGroup(job);
                open.Add(group);
            }
            else
            {
                yield return new JobGroup([job]);
                continue;
            }

            if (group.Count < maxFiles)
                continue;

            open.Remove(group);
            yield return group.ToJobGroup();
        }

        foreach (var group in open)
            yield return group.ToJobGroup();
    }

    private static bool IsSmall(PngJob job) => job.File.Size <= SmallFileLimit;

    private sealed class OpenGroup
    {
        private readonly string _folder;
        private readonly string _volume;
        private readonly List<PngJob> _jobs = [];
        private readonly HashSet<string> _names = new(StringComparer.OrdinalIgnoreCase);
        private long _size;

        public OpenGroup(PngJob host)
        {
            _folder = FolderOf(host);
            _volume = Path.GetPathRoot(host.TargetPath) ?? string.Empty;
            Add(host);
        }

        public int Count => _jobs.Count;

        public static bool CanHost(PngJob job) => TempPaths.FitsResult(FolderOf(job), job.File.FullPath);

        public bool Accepts(PngJob job, int maxFiles) =>
            _jobs.Count < maxFiles
            && _size + job.File.Size <= MaxGroupSize
            && string.Equals(_volume, Path.GetPathRoot(job.TargetPath), StringComparison.OrdinalIgnoreCase)
            && !_names.Contains(Path.GetFileName(job.File.FullPath))
            && TempPaths.FitsResult(_folder, job.File.FullPath);

        public void Add(PngJob job)
        {
            _jobs.Add(job);
            _names.Add(Path.GetFileName(job.File.FullPath));
            _size += job.File.Size;
        }

        public JobGroup ToJobGroup() => new([.. _jobs]);

        private static string FolderOf(PngJob job) => Path.GetDirectoryName(job.TargetPath) ?? string.Empty;
    }
}
