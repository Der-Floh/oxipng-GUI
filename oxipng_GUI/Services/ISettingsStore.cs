using OxipngGui.Models;

namespace OxipngGui.Services;

public interface ISettingsStore
{
    UserSettings Load();

    void Save(UserSettings settings);
}
