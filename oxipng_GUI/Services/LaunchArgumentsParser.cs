using System.CommandLine;
using System.CommandLine.Help;
using System.CommandLine.Parsing;
using System.Globalization;
using System.Text.RegularExpressions;

using OxipngGui.Models;

namespace OxipngGui.Services;

/// <summary>
/// Reads the command line. Paths are added to the list. oxipng's own options set up the window for this launch,
/// starting from oxipng's defaults as if oxipng itself were run with them; those without a control in the window
/// are passed to oxipng unchanged.
/// </summary>
public sealed class LaunchArgumentsParser
{
    private const int HelpWidth = 100;
    private const string MaxLevelName = "max";
    private const string StripSafeName = "safe";
    private const string StripAllName = "all";
    private const string StandardInputPath = "-";
    private const string Description =
        "Optimizes PNG files with oxipng without changing a single pixel.\n\n"
        + "Paths are added to the list. oxipng options set up the window for this launch only: it starts from oxipng's defaults plus the options you pass, as if you ran oxipng with them, and nothing is saved. "
        + "Options the window has no control for are passed to oxipng unchanged. -r, -q, -v, -j, -t and --sequential are accepted and have no effect.";

    private static readonly Regex ChunkList = new("^[A-Za-z]{4}(,[A-Za-z]{4})*$", RegexOptions.CultureInvariant);
    private static readonly Regex KeepList = new("^([A-Za-z]{4}|display)(,([A-Za-z]{4}|display))*$", RegexOptions.CultureInvariant);
    private static readonly Regex FilterList = new("^[0-9](-[0-9])?(,[0-9](-[0-9])?)*$", RegexOptions.CultureInvariant);
    private static readonly Regex SizeValue = new(@"^[0-9]+(\.[0-9]+)?\s*[A-Za-z]*$", RegexOptions.CultureInvariant);

    private readonly Argument<string[]> _paths = new("paths")
    {
        Description = "PNG files or folders to add. Folders include their subfolders.",
        Arity = ArgumentArity.ZeroOrMore,
    };

    private readonly Option<string> _level = Choice("--opt", "level", "Optimization level, 0-6 or max [default: 2].", ["0", "1", "2", "3", "4", "5", "6", MaxLevelName], "-o");
    private readonly Option<bool> _stripSafe = Flag("-s", "Strip safely-removable metadata, same as '--strip safe'.");
    private readonly Option<string> _strip = Matching("--strip", "mode", "Strip metadata: safe, all, or a comma-separated list of chunks such as bKGD,cHRM.", value => value is StripSafeName or StripAllName || ChunkList.IsMatch(value));
    private readonly Option<string> _keep = Matching("--keep", "list", "Strip all metadata except the listed chunks. 'display' keeps the chunks that affect how the image looks.", KeepList.IsMatch);
    private readonly Option<bool> _alpha = Flag("--alpha", "Optimize fully transparent pixels by changing their hidden color.", "-a");
    private readonly Option<string> _interlace = Choice("--interlace", "mode", "off removes interlacing [default], on adds Adam7 interlacing, keep leaves it as it is.", ["off", "on", "keep", "0", "1"], "-i");
    private readonly Option<bool> _scale16 = Flag("--scale16", "Reduce 16-bit images to 8-bit. This is lossy.");
    private readonly Option<bool> _preserve = Flag("--preserve", "Keep file dates.", "-p");
    private readonly Option<bool> _fix = Flag("--fix", "Accept files with bad chunk checksums. The output always gets correct ones.");
    private readonly Option<bool> _zopfli = Flag("--zopfli", "Use the much slower Zopfli compressor.", "-z", "-Z");
    private readonly Option<int> _timeout = Number("--timeout", "secs", "Stop trying new strategies on a file after this many seconds (0-3600).", 0, OptimizationOptions.MaxTimeoutSeconds);
    private readonly Option<string> _dir = Folder("--dir", "directory", "Save optimized copies to this folder instead of overwriting the originals. Files from added folders keep their subfolders.");

    private readonly Option<int> _compression = Number("--zc", "level", "Deflate level 0-12 for the main compression trials.", 0, 12);
    private readonly Option<string> _filters = Matching("--filters", "list", "Filters to try, for example 0,5 or 0-9.", FilterList.IsMatch, "-f");
    private readonly Option<bool> _fast = Flag("--fast", "Evaluate each filter quickly, then run one main compression trial.");
    private readonly Option<bool> _noBitDepth = Flag("--nb", "Don't change the bit depth.");
    private readonly Option<bool> _noColorType = Flag("--nc", "Don't change the color type.");
    private readonly Option<bool> _noPalette = Flag("--np", "Don't convert to indexed colors or change an existing palette.");
    private readonly Option<bool> _noGrayscale = Flag("--ng", "Don't change to or from grayscale.");
    private readonly Option<bool> _noTransformations = Flag("--nx", "Don't transform the image at all; interlacing is kept unless -i says otherwise.");
    private readonly Option<bool> _noRecompression = Flag("--nz", "Don't recompress image data unless a transformation requires it.");
    private readonly Option<bool> _force = Flag("--force", "Write the result even if it isn't smaller than the original.");
    private readonly Option<int> _zopfliIterations = Number("--zi", "iterations", "Zopfli iterations [default: 15]. Needs -z.", 1, int.MaxValue);
    private readonly Option<int> _zopfliStop = Number("--ziwi", "iterations", "Stop Zopfli after this many iterations without improvement. Needs -z.", 1, int.MaxValue);
    private readonly Option<int> _bruteLevel = Number("--brute-level", "level", "Compression level 1-12 for the Brute filter.", 1, 12);
    private readonly Option<int> _bruteLines = Number("--brute-lines", "lines", "Lines the Brute filter compresses at once.", 1, int.MaxValue);
    private readonly Option<string> _maxRawSize = Matching("--max-raw-size", "bytes", "Skip images larger than this, for example 10MB.", SizeValue.IsMatch);

    private readonly Option<bool> _recursive = Ignored("--recursive", "-r");
    private readonly Option<bool> _quiet = Ignored("--quiet", "-q");
    private readonly Option<bool> _verbose = Ignored("--verbose", "-v");
    private readonly Option<bool> _json = Ignored("--json", "-j");
    private readonly Option<int> _threads = IgnoredNumber("--threads", "-t");
    private readonly Option<bool> _sequential = Ignored("--sequential");

    private readonly Option<string> _out = UnsupportedValue("--out", "--out isn't supported, because oxipng GUI writes every file itself. Use --dir to save copies to a folder.");
    private readonly Option<bool> _stdout = Unsupported("--stdout", "--stdout isn't supported, because oxipng GUI has no console output.");
    private readonly Option<bool> _dryRun = Unsupported("--dry-run", "--dry-run isn't supported. Use --dir to try settings on copies.", "-d");

    private readonly Option<bool> _run = Flag("--run", "Start optimizing as soon as the files are listed (oxipng GUI only).");
    private readonly Option<bool> _version = Flag("--version", "Show the version.", "-V");

    private readonly Option[] _oxipngOptions;
    private readonly RootCommand _command;

    public LaunchArgumentsParser()
    {
        _oxipngOptions =
        [
            _level, _stripSafe, _strip, _keep, _alpha, _interlace, _scale16, _preserve, _fix, _zopfli, _timeout, _dir,
            _compression, _filters, _fast, _noBitDepth, _noColorType, _noPalette, _noGrayscale, _noTransformations, _noRecompression,
            _force, _zopfliIterations, _zopfliStop, _bruteLevel, _bruteLines, _maxRawSize,
            _recursive, _quiet, _verbose, _json, _threads, _sequential, _out, _stdout, _dryRun,
        ];
        _command = CreateCommand();
    }

    public LaunchArguments Parse(IReadOnlyList<string> args)
    {
        if (args is null)
            throw new ArgumentNullException(nameof(args));

        var result = _command.Parse(args, new ParserConfiguration { ResponseFileTokenReplacer = null });
        if (result.Action is HelpAction)
            return LaunchArguments.Help(HelpText(result));
        if (result.Errors.Count > 0)
            return LaunchArguments.Invalid([.. result.Errors.Select(error => error.Message)]);
        if (result.GetValue(_version))
            return LaunchArguments.Version();

        var request = new LaunchRequest(result.GetValue(_paths) ?? [], result.GetValue(_run));
        var givesOxipngOptions = _oxipngOptions.Any(option => IsGiven(result, option));
        return LaunchArguments.Start(request, givesOxipngOptions ? settings => ApplyTo(result, settings) : null);
    }

    private RootCommand CreateCommand()
    {
        var command = new RootCommand(Description) { _paths };
        foreach (var builtInVersion in command.Options.OfType<VersionOption>().ToList())
            command.Options.Remove(builtInVersion);
        foreach (var option in _oxipngOptions)
            command.Options.Add(option);
        command.Options.Add(_run);
        command.Options.Add(_version);
        command.Directives.Clear();
        foreach (var help in command.Options.OfType<HelpOption>())
        {
            if (help.Action is HelpAction helpAction)
                helpAction.MaxWidth = HelpWidth;
        }

        command.Validators.Add(ValidateCombinations);
        return command;
    }

    private void ValidateCombinations(CommandResult result)
    {
        if (IsGiven(result, _stripSafe) && IsGiven(result, _strip))
            result.AddError("-s can't be combined with --strip.");
        if (IsGiven(result, _keep) && (IsGiven(result, _strip) || IsGiven(result, _stripSafe)))
            result.AddError("--keep can't be combined with --strip or -s.");
        if (IsGiven(result, _zopfli) && IsGiven(result, _compression))
            result.AddError("--zc can't be combined with -z/--zopfli.");
        if (!IsGiven(result, _zopfli) && (IsGiven(result, _zopfliIterations) || IsGiven(result, _zopfliStop)))
            result.AddError("--zi and --ziwi need -z/--zopfli.");
        var paths = result.GetValue(_paths) ?? [];
        if (paths.Contains(StandardInputPath, StringComparer.Ordinal))
            result.AddError("Reading from standard input ('-') isn't supported. Pass file or folder paths instead.");
        foreach (var unknown in paths.Where(path => path.Length > StandardInputPath.Length && path.StartsWith(StandardInputPath, StringComparison.Ordinal)))
            result.AddError($"Unknown option: {unknown}");
    }

    private UserSettings ApplyTo(ParseResult result, UserSettings settings)
    {
        var options = new OptimizationOptions(
            level: LevelOf(result.GetValue(_level)),
            metadata: MetadataOf(result),
            useZopfli: result.GetValue(_zopfli),
            optimizeAlpha: result.GetValue(_alpha),
            keepFileDates: result.GetValue(_preserve),
            interlace: InterlaceOf(result),
            timeoutSeconds: result.GetValue(_timeout),
            fixChecksums: result.GetValue(_fix),
            reduceTo8Bit: result.GetValue(_scale16),
            extra: ExtraOf(result),
            forceWrite: result.GetValue(_force));

        var outputFolder = result.GetValue(_dir);
        return new UserSettings(
            options,
            saveToFolder: outputFolder is not null,
            outputFolder: outputFolder is null ? settings.OutputFolder : Path.GetFullPath(outputFolder),
            showAdvanced: settings.ShowAdvanced || UsesAdvancedOptions(options),
            lastBrowseFolder: settings.LastBrowseFolder);
    }

    private MetadataMode MetadataOf(ParseResult result)
    {
        if (result.GetValue(_stripSafe))
            return MetadataMode.StripSafe;

        return result.GetValue(_strip) switch
        {
            StripSafeName => MetadataMode.StripSafe,
            StripAllName => MetadataMode.StripAll,
            _ => MetadataMode.Keep,
        };
    }

    private InterlaceMode InterlaceOf(ParseResult result) => result.GetValue(_interlace) switch
    {
        "off" or "0" => InterlaceMode.Remove,
        "on" or "1" => InterlaceMode.Add,
        "keep" => InterlaceMode.Keep,
        _ => result.GetValue(_noTransformations) ? InterlaceMode.Keep : InterlaceMode.Remove,
    };

    private ExtraOxipngOptions ExtraOf(ParseResult result)
    {
        List<string> arguments = [];
        AddValue(arguments, result, _compression);
        AddValue(arguments, result, _filters);
        foreach (var flag in new[] { _fast, _noBitDepth, _noColorType, _noPalette, _noGrayscale, _noTransformations, _noRecompression })
        {
            if (result.GetValue(flag))
                arguments.Add(flag.Name);
        }

        AddValue(arguments, result, _zopfliIterations);
        AddValue(arguments, result, _zopfliStop);
        AddValue(arguments, result, _bruteLevel);
        AddValue(arguments, result, _bruteLines);
        AddValue(arguments, result, _maxRawSize);
        var stripsChunkList = result.GetValue(_strip) is string strip && strip is not (StripSafeName or StripAllName);
        if (stripsChunkList)
            AddValue(arguments, result, _strip);
        AddValue(arguments, result, _keep);

        if (arguments.Count == 0)
            return ExtraOxipngOptions.None;

        bool? requiredZopfli = IsGiven(result, _zopfliIterations) || IsGiven(result, _zopfliStop) ? true
            : IsGiven(result, _compression) ? false
            : null;
        return new ExtraOxipngOptions(arguments, controlsMetadata: stripsChunkList || IsGiven(result, _keep), requiredZopfli: requiredZopfli);
    }

    private static int LevelOf(string? value) => value switch
    {
        null => OptimizationOptions.DefaultLevel,
        MaxLevelName => OptimizationOptions.MaxLevel,
        _ => int.Parse(value, CultureInfo.InvariantCulture),
    };

    private static bool UsesAdvancedOptions(OptimizationOptions options) =>
        options.Interlace != InterlaceMode.Remove || options.TimeoutSeconds > 0 || options.FixChecksums || options.ReduceTo8Bit
        || options.ForceWrite || options.Extra.Arguments.Count > 0;

    private static void AddValue<T>(List<string> arguments, ParseResult result, Option<T> option)
    {
        if (!IsGiven(result, option))
            return;

        arguments.Add(option.Name);
        arguments.Add(Convert.ToString(result.GetValue(option), CultureInfo.InvariantCulture));
    }

    // Flags get an implicit result even when they aren't on the command line.
    private static bool IsGiven(CommandResult result, Option option) => result.GetResult(option) is { Implicit: false };

    private static bool IsGiven(ParseResult result, Option option) => result.GetResult(option) is { Implicit: false };

    private static string HelpText(ParseResult result)
    {
        using var writer = new StringWriter(CultureInfo.InvariantCulture);
        result.Invoke(new InvocationConfiguration { Output = writer, Error = writer });
        return writer.ToString();
    }

    private static Option<bool> Flag(string name, string description, params string[] aliases) =>
        new(name, aliases) { Description = description, Arity = ArgumentArity.Zero };

    private static Option<string> Choice(string name, string helpName, string description, string[] values, params string[] aliases)
    {
        var option = new Option<string>(name, aliases) { Description = description, HelpName = helpName };
        option.AcceptOnlyFromAmong(values);
        return option;
    }

    private static Option<string> Matching(string name, string helpName, string description, Func<string, bool> isValid, params string[] aliases)
    {
        var option = new Option<string>(name, aliases) { Description = description, HelpName = helpName };
        option.Validators.Add(result =>
        {
            if (result.GetValue(option) is not string value || !isValid(value))
                result.AddError($"'{result.GetValue(option)}' isn't a valid value for {name}.");
        });
        return option;
    }

    private static Option<int> Number(string name, string helpName, string description, int min, int max)
    {
        var option = new Option<int>(name) { Description = description, HelpName = helpName };
        option.Validators.Add(result =>
        {
            var value = result.GetValue(option);
            if (value < min || value > max)
                result.AddError(max == int.MaxValue ? $"{name} must be at least {min}." : $"{name} must be between {min} and {max}.");
        });
        return option;
    }

    private static Option<string> Folder(string name, string helpName, string description)
    {
        var option = new Option<string>(name) { Description = description, HelpName = helpName };
        option.Validators.Add(result =>
        {
            try
            {
                Path.GetFullPath(result.GetValue(option) ?? string.Empty);
            }
            catch (Exception ex) when (ex is ArgumentException or NotSupportedException or PathTooLongException)
            {
                result.AddError($"'{result.GetValue(option)}' isn't a valid folder for {name}.");
            }
        });
        return option;
    }

    private static Option<bool> Ignored(string name, params string[] aliases) =>
        new(name, aliases) { Hidden = true, Arity = ArgumentArity.Zero, AllowMultipleArgumentsPerToken = true };

    private static Option<int> IgnoredNumber(string name, params string[] aliases) => new(name, aliases) { Hidden = true };

    private static Option<bool> Unsupported(string name, string reason, params string[] aliases)
    {
        var option = new Option<bool>(name, aliases) { Hidden = true, Arity = ArgumentArity.Zero };
        option.Validators.Add(result =>
        {
            if (!result.Implicit)
                result.AddError(reason);
        });
        return option;
    }

    private static Option<string> UnsupportedValue(string name, string reason)
    {
        var option = new Option<string>(name) { Hidden = true };
        option.Validators.Add(result => result.AddError(reason));
        return option;
    }
}
