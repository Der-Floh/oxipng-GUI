using OxipngGui.Models;

namespace OxipngGui.Services;

/// <summary>
/// The settings for a launch with oxipng options on the command line. They're applied on top of the saved settings
/// and nothing is saved, so the next normal start looks as before.
/// </summary>
public sealed class LaunchSettingsStore(ISettingsStore savedSettings, LaunchArguments launch) : ISettingsStore
{
    private readonly ISettingsStore _savedSettings = savedSettings ?? throw new ArgumentNullException(nameof(savedSettings));
    private readonly LaunchArguments _launch = launch ?? throw new ArgumentNullException(nameof(launch));

    public UserSettings Load() => _launch.ApplyTo(_savedSettings.Load());

    public void Save(UserSettings settings) { }
}
