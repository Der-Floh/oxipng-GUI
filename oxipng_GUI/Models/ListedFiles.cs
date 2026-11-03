namespace OxipngGui.Models;

/// <summary>The files in the list, in list order, each with its latest result.</summary>
public sealed class ListedFiles
{
    private readonly List<FileResult> _results = [];
    private readonly Dictionary<string, int> _indexByPath = new(StringComparer.OrdinalIgnoreCase);

    public int Count => _results.Count;

    public FileResult this[int index] => _results[index];

    /// <summary>Adds the file as waiting. A file that's already listed goes back to waiting instead.</summary>
    /// <returns><see langword="true"/> if the file wasn't listed yet.</returns>
    public bool Add(PngFile file)
    {
        if (file is null)
            throw new ArgumentNullException(nameof(file));

        if (_indexByPath.TryGetValue(file.FullPath, out var index))
        {
            _results[index] = FileResult.Waiting(file);
            return false;
        }

        Append(FileResult.Waiting(file));
        return true;
    }

    public void SetWaiting(int index) => _results[index] = FileResult.Waiting(_results[index].File);

    /// <summary>Replaces the result of the file that <paramref name="result"/> is about.</summary>
    /// <returns><see langword="false"/> if that file isn't listed.</returns>
    public bool TryUpdate(FileResult result, out int index)
    {
        if (result is null)
            throw new ArgumentNullException(nameof(result));

        if (!_indexByPath.TryGetValue(result.File.FullPath, out index))
            return false;

        _results[index] = result;
        return true;
    }

    public void RemoveAt(IEnumerable<int> indexes)
    {
        if (indexes is null)
            throw new ArgumentNullException(nameof(indexes));

        var removed = new HashSet<int>(indexes);
        List<FileResult> kept = [.. _results.Where((_, index) => !removed.Contains(index))];
        Clear();
        foreach (var result in kept)
            Append(result);
    }

    public void Clear()
    {
        _results.Clear();
        _indexByPath.Clear();
    }

    /// <summary>
    /// Finds the first file, from <paramref name="startIndex"/> down and then from the top, whose path in the list starts
    /// with <paramref name="prefix"/>, ignoring case.
    /// </summary>
    /// <returns>The file's index, or -1 if none matches.</returns>
    public int FindByPrefix(string prefix, int startIndex)
    {
        if (prefix is null)
            throw new ArgumentNullException(nameof(prefix));

        var start = startIndex >= 0 && startIndex < _results.Count ? startIndex : 0;
        for (var offset = 0; offset < _results.Count; offset++)
        {
            var index = (start + offset) % _results.Count;
            if (_results[index].File.RelativeOutputPath.StartsWith(prefix, StringComparison.CurrentCultureIgnoreCase))
                return index;
        }

        return -1;
    }

    public override string ToString() => $"{Count} files";

    private void Append(FileResult result)
    {
        _indexByPath.Add(result.File.FullPath, _results.Count);
        _results.Add(result);
    }
}
