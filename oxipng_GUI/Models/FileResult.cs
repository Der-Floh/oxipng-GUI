namespace OxipngGui.Models;

public sealed class FileResult
{
    private FileResult(PngFile file, FileStatus status, long originalSize, long? optimizedSize, string? error)
    {
        File = file;
        Status = status;
        OriginalSize = originalSize;
        OptimizedSize = optimizedSize;
        Error = error;
    }

    public PngFile File { get; }

    public FileStatus Status { get; }

    public long OriginalSize { get; }

    public long? OptimizedSize { get; }

    public string? Error { get; }

    public bool IsFinished => Status is FileStatus.Optimized or FileStatus.AlreadyOptimized;

    public long SavedBytes => OptimizedSize is long optimizedSize ? OriginalSize - optimizedSize : 0;

    public static FileResult Waiting(PngFile file) => new(file, FileStatus.Waiting, file.Size, null, null);

    public static FileResult Optimizing(PngFile file) => new(file, FileStatus.Optimizing, file.Size, null, null);

    public static FileResult Optimized(PngFile file, long originalSize, long optimizedSize) =>
        new(file, FileStatus.Optimized, originalSize, optimizedSize, null);

    public static FileResult AlreadyOptimized(PngFile file, long originalSize) =>
        new(file, FileStatus.AlreadyOptimized, originalSize, originalSize, null);

    public static FileResult Failed(PngFile file, string error) => new(file, FileStatus.Failed, file.Size, null, error);

    public static FileResult Stopped(PngFile file) => new(file, FileStatus.Stopped, file.Size, null, null);

    public override string ToString() => Error is null ? $"{File}: {Status}" : $"{File}: {Status} ({Error})";
}
