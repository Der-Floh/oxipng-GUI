using System.Reflection;

using OxipngGui.Models;

namespace OxipngGui.Services;

public static class AboutInfoFactory
{
    private const char BuildMetadataSeparator = '+';
    private const string UnknownVersion = "unknown";

    public static AboutInfo Create(string oxipngVersion)
    {
        if (string.IsNullOrWhiteSpace(oxipngVersion))
            throw new ArgumentException("The oxipng version must not be empty.", nameof(oxipngVersion));

        return new AboutInfo(AppVersion(), oxipngVersion, LicenseText());
    }

    private static string AppVersion()
    {
        var version = typeof(AboutInfoFactory).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? UnknownVersion;
        var metadataStart = version.IndexOf(BuildMetadataSeparator);
        return metadataStart < 0 ? version : version.Substring(0, metadataStart);
    }

    private static string LicenseText()
    {
        var sections = new[]
        {
            Section("oxipng GUI", EmbeddedResources.ReadText(EmbeddedResources.AppLicense)),
            Section("oxipng", EmbeddedResources.ReadText(EmbeddedResources.OxipngLicense)),
            EmbeddedResources.ReadText(EmbeddedResources.ThirdPartyNotices).Trim(),
        };

        return NormalizeLineEndings(string.Join("\n\n\n", sections));
    }

    private static string Section(string title, string text) => $"{title}\n{new string('=', title.Length)}\n\n{text.Trim()}";

    private static string NormalizeLineEndings(string text) => text.Replace("\r\n", "\n").Replace("\n", Environment.NewLine);
}
