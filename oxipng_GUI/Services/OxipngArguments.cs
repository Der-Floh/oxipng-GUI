using System.Globalization;
using System.Text;

using OxipngGui.Models;

namespace OxipngGui.Services;

public static class OxipngArguments
{
    public static IReadOnlyList<string> OptionFlags(OptimizationOptions options)
    {
        if (options is null)
            throw new ArgumentNullException(nameof(options));

        List<string> flags = ["--opt", Invariant(options.Level)];
        if (options.Metadata != MetadataMode.Keep)
            flags.AddRange(["--strip", StripValue(options.Metadata)]);
        if (options.UseZopfli)
            flags.Add("--zopfli");
        if (options.OptimizeAlpha)
            flags.Add("--alpha");
        if (options.KeepFileDates)
            flags.Add("--preserve");
        if (options.Interlace != InterlaceMode.Remove)
            flags.AddRange(["--interlace", InterlaceValue(options.Interlace)]);
        if (options.TimeoutSeconds > 0)
            flags.AddRange(["--timeout", Invariant(options.TimeoutSeconds)]);
        if (options.FixChecksums)
            flags.Add("--fix");
        if (options.ReduceTo8Bit)
            flags.Add("--scale16");

        return flags;
    }

    public static IReadOnlyList<string> ForFile(OptimizationOptions options, int threads, string inputPath, string outputPath)
    {
        if (threads < 1)
            throw new ArgumentOutOfRangeException(nameof(threads), threads, "At least one thread is needed.");
        if (string.IsNullOrEmpty(inputPath))
            throw new ArgumentException("The input path must not be empty.", nameof(inputPath));
        if (string.IsNullOrEmpty(outputPath))
            throw new ArgumentException("The output path must not be empty.", nameof(outputPath));

        return [.. OptionFlags(options), "--threads", Invariant(threads), "--quiet", "--json", "--out", outputPath, EscapeGlobPattern(inputPath)];
    }

    private static string StripValue(MetadataMode mode) => mode switch
    {
        MetadataMode.StripSafe => "safe",
        MetadataMode.StripAll => "all",
        _ => throw new NotSupportedException($"Unsupported metadata mode: {mode}"),
    };

    private static string InterlaceValue(InterlaceMode mode) => mode switch
    {
        InterlaceMode.Keep => "keep",
        InterlaceMode.Add => "on",
        _ => throw new NotSupportedException($"Unsupported interlace mode: {mode}"),
    };

    private static string Invariant(int value) => value.ToString(CultureInfo.InvariantCulture);

    // On Windows oxipng expands input paths as glob patterns, so "[1].png" would otherwise match "1.png".
    private static string EscapeGlobPattern(string path)
    {
        var escaped = new StringBuilder(path.Length);
        foreach (var c in path)
        {
            if (c is '[' or ']')
                escaped.Append('[').Append(c).Append(']');
            else
                escaped.Append(c);
        }

        return escaped.ToString();
    }
}
