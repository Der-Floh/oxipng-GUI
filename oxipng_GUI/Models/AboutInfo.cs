namespace OxipngGui.Models;

public sealed class AboutInfo(string appVersion, string oxipngVersion, string licenseText)
{
    public string AppVersion { get; } = appVersion;

    public string OxipngVersion { get; } = oxipngVersion;

    public string LicenseText { get; } = licenseText;

    public override string ToString() => $"oxipng GUI {AppVersion} (oxipng {OxipngVersion})";
}
