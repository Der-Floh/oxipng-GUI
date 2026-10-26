namespace OxipngGui.Models;

public sealed class BatchSummary
{
    private BatchSummary(int optimized, int alreadyOptimized, int failed, long originalBytes, long optimizedBytes)
    {
        Optimized = optimized;
        AlreadyOptimized = alreadyOptimized;
        Failed = failed;
        OriginalBytes = originalBytes;
        OptimizedBytes = optimizedBytes;
    }

    public static BatchSummary Empty { get; } = new(0, 0, 0, 0, 0);

    public int Optimized { get; }

    public int AlreadyOptimized { get; }

    public int Failed { get; }

    public long OriginalBytes { get; }

    public long OptimizedBytes { get; }

    public int Done => Optimized + AlreadyOptimized + Failed;

    public long SavedBytes => OriginalBytes - OptimizedBytes;

    public double SavedRatio => OriginalBytes == 0 ? 0 : (double)SavedBytes / OriginalBytes;

    public BatchSummary Add(FileResult result) => result.Status switch
    {
        FileStatus.Optimized => new(Optimized + 1, AlreadyOptimized, Failed, OriginalBytes + result.OriginalSize, OptimizedBytes + result.OptimizedSize.GetValueOrDefault(result.OriginalSize)),
        FileStatus.AlreadyOptimized => new(Optimized, AlreadyOptimized + 1, Failed, OriginalBytes + result.OriginalSize, OptimizedBytes + result.OriginalSize),
        FileStatus.Failed => new(Optimized, AlreadyOptimized, Failed + 1, OriginalBytes, OptimizedBytes),
        FileStatus.Waiting or FileStatus.Optimizing or FileStatus.Stopped => this,
        _ => throw new NotSupportedException($"Unsupported file status: {result.Status}"),
    };

    public override string ToString() => $"{Optimized} optimized, {AlreadyOptimized} already optimized, {Failed} failed, {SavedBytes} bytes saved";
}
