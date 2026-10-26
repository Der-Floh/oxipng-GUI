namespace OxipngGui.Services;

public interface IOxipngBinary
{
    string Version { get; }

    /// <summary>
    /// Extracts the embedded oxipng.exe if needed, checks once that it starts and returns its path.
    /// </summary>
    /// <exception cref="OxipngUnavailableException">oxipng can't be extracted or started on this PC.</exception>
    string EnsureAvailable();
}
