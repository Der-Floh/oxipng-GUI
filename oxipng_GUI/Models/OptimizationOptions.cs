namespace OxipngGui.Models;

/// <param name="extra">oxipng options from the command line that have no control in the window. Never saved.</param>
/// <param name="forceWrite">
/// <c>--force</c> from the command line: write the result even if it isn't smaller. Never saved.
/// </param>
public sealed class OptimizationOptions(
    int level,
    MetadataMode metadata,
    bool useZopfli,
    bool optimizeAlpha,
    bool keepFileDates,
    InterlaceMode interlace,
    int timeoutSeconds,
    bool fixChecksums,
    bool reduceTo8Bit,
    ExtraOxipngOptions? extra = null,
    bool forceWrite = false)
{
    public const int MinLevel = 0;
    public const int MaxLevel = 6;
    public const int DefaultLevel = 2;
    public const int MaxTimeoutSeconds = 3600;

    public int Level { get; } = level is >= MinLevel and <= MaxLevel
        ? level
        : throw new ArgumentOutOfRangeException(nameof(level), level, $"The optimization level must be between {MinLevel} and {MaxLevel}.");

    public MetadataMode Metadata { get; } = metadata;

    public bool UseZopfli { get; } = useZopfli;

    public bool OptimizeAlpha { get; } = optimizeAlpha;

    public bool KeepFileDates { get; } = keepFileDates;

    public InterlaceMode Interlace { get; } = interlace;

    public int TimeoutSeconds { get; } = timeoutSeconds is >= 0 and <= MaxTimeoutSeconds
        ? timeoutSeconds
        : throw new ArgumentOutOfRangeException(nameof(timeoutSeconds), timeoutSeconds, $"The timeout must be between 0 and {MaxTimeoutSeconds} seconds.");

    public bool FixChecksums { get; } = fixChecksums;

    public bool ReduceTo8Bit { get; } = reduceTo8Bit;

    public ExtraOxipngOptions Extra { get; } = extra ?? ExtraOxipngOptions.None;

    public bool ForceWrite { get; } = forceWrite;

    public override string ToString() =>
        $"Level {Level}, metadata {Metadata}, interlacing {Interlace}, Zopfli {UseZopfli}, alpha {OptimizeAlpha}, keep dates {KeepFileDates}, timeout {TimeoutSeconds}s, fix {FixChecksums}, 16-to-8-bit {ReduceTo8Bit}, force {ForceWrite}, extra [{Extra}]";
}
