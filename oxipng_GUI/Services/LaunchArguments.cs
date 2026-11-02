using OxipngGui.Models;

namespace OxipngGui.Services;

/// <summary>The parsed command line: what the window opens with, or what to show instead of opening it.</summary>
public sealed class LaunchArguments
{
    private readonly Func<UserSettings, UserSettings>? _settingsOverride;

    private LaunchArguments(LaunchRequest request, Func<UserSettings, UserSettings>? settingsOverride, string? helpText, bool showVersion, IReadOnlyList<string> errors)
    {
        Request = request;
        _settingsOverride = settingsOverride;
        HelpText = helpText;
        ShowVersion = showVersion;
        Errors = errors;
    }

    public LaunchRequest Request { get; }

    public string? HelpText { get; }

    public bool ShowVersion { get; }

    public IReadOnlyList<string> Errors { get; }

    /// <summary>Whether oxipng options were given, which replace the saved settings for this launch.</summary>
    public bool OverridesSettings => _settingsOverride is not null;

    public UserSettings ApplyTo(UserSettings settings)
    {
        if (settings is null)
            throw new ArgumentNullException(nameof(settings));

        return _settingsOverride is null ? settings : _settingsOverride(settings);
    }

    public static LaunchArguments Start(LaunchRequest request, Func<UserSettings, UserSettings>? settingsOverride) =>
        new(request ?? throw new ArgumentNullException(nameof(request)), settingsOverride, null, false, []);

    public static LaunchArguments Help(string helpText) => new(LaunchRequest.None, null, helpText, false, []);

    public static LaunchArguments Version() => new(LaunchRequest.None, null, null, true, []);

    public static LaunchArguments Invalid(IReadOnlyList<string> errors) => new(LaunchRequest.None, null, null, false, errors);

    public override string ToString() =>
        Errors.Count > 0 ? $"Invalid: {string.Join(" ", Errors)}"
        : HelpText is not null ? "Help"
        : ShowVersion ? "Version"
        : $"{Request}, overrides settings: {OverridesSettings}";
}
