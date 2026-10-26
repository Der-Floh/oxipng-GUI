namespace OxipngGui.Models;

public sealed class OptimizationOptions(
    int level,
    MetadataMode metadata,
    bool useZopfli,
    bool optimizeAlpha,
    bool keepFileDates,
    InterlaceMode interlace,
    int timeoutSeconds,
    bool fixChecksums,
    bool reduceTo8Bit)
{
    public const int MinLevel = 0;
    public const int MaxLevel = 6;
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

    public override string ToString() =>
        $"Level {Level}, metadata {Metadata}, interlacing {Interlace}, Zopfli {UseZopfli}, alpha {OptimizeAlpha}, keep dates {KeepFileDates}, timeout {TimeoutSeconds}s, fix {FixChecksums}, 16-to-8-bit {ReduceTo8Bit}";
}
