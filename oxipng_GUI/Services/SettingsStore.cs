using System.Configuration;

using OxipngGui.Models;
using OxipngGui.Properties;
using OxipngGui.Utils;

namespace OxipngGui.Services;

/// <summary>
/// Reads and writes the per-user settings. Settings carry over to new versions, and a corrupt settings file is
/// replaced with the defaults instead of keeping the app from starting.
/// </summary>
internal sealed class SettingsStore(Settings settings) : ISettingsStore
{
    private readonly Settings _settings = settings ?? throw new ArgumentNullException(nameof(settings));

    public UserSettings Load()
    {
        RecoverFromCorruptFile();
        UpgradeOnce();
        return new UserSettings(
            new OptimizationOptions(
                level: Clamp(_settings.OptimizationLevel, OptimizationOptions.MinLevel, OptimizationOptions.MaxLevel),
                metadata: ParseEnum(_settings.MetadataMode, MetadataMode.Keep),
                useZopfli: _settings.UseZopfli,
                optimizeAlpha: _settings.OptimizeAlpha,
                keepFileDates: _settings.KeepFileDates,
                interlace: ParseEnum(_settings.InterlaceMode, InterlaceMode.Remove),
                timeoutSeconds: Clamp(_settings.TimeoutSeconds, 0, OptimizationOptions.MaxTimeoutSeconds),
                fixChecksums: _settings.FixChecksums,
                reduceTo8Bit: _settings.ReduceTo8Bit),
            saveToFolder: _settings.SaveToFolder,
            outputFolder: _settings.OutputFolder ?? string.Empty,
            showAdvanced: _settings.ShowAdvanced,
            lastBrowseFolder: _settings.LastBrowseFolder ?? string.Empty);
    }

    public void Save(UserSettings settings)
    {
        if (settings is null)
            throw new ArgumentNullException(nameof(settings));

        var options = settings.Options;
        _settings.OptimizationLevel = options.Level;
        _settings.MetadataMode = options.Metadata.ToString();
        _settings.UseZopfli = options.UseZopfli;
        _settings.OptimizeAlpha = options.OptimizeAlpha;
        _settings.KeepFileDates = options.KeepFileDates;
        _settings.InterlaceMode = options.Interlace.ToString();
        _settings.TimeoutSeconds = options.TimeoutSeconds;
        _settings.FixChecksums = options.FixChecksums;
        _settings.ReduceTo8Bit = options.ReduceTo8Bit;
        _settings.SaveToFolder = settings.SaveToFolder;
        _settings.OutputFolder = settings.OutputFolder;
        _settings.ShowAdvanced = settings.ShowAdvanced;
        _settings.LastBrowseFolder = settings.LastBrowseFolder;
        TrySave();
    }

    private void RecoverFromCorruptFile()
    {
        try
        {
            _ = _settings.UpgradeRequired;
        }
        catch (ConfigurationErrorsException ex)
        {
            FileHelper.TryDelete(ex.Filename ?? (ex.InnerException as ConfigurationErrorsException)?.Filename);
            _settings.Reload();
        }
    }

    private void UpgradeOnce()
    {
        if (!_settings.UpgradeRequired)
            return;

        _settings.Upgrade();
        _settings.UpgradeRequired = false;
        TrySave();
    }

    private void TrySave()
    {
        try
        {
            _settings.Save();
        }
        catch (ConfigurationErrorsException)
        {
            // Losing a preference must not interrupt the user's work.
        }
    }

    private static int Clamp(int value, int min, int max) => Math.Max(min, Math.Min(max, value));

    private static TEnum ParseEnum<TEnum>(string? value, TEnum fallback) where TEnum : struct, Enum =>
        Enum.TryParse<TEnum>(value, ignoreCase: false, out var parsed) && Enum.IsDefined(typeof(TEnum), parsed) ? parsed : fallback;
}
