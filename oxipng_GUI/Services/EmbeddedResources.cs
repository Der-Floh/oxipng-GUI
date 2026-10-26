using System.Reflection;

namespace OxipngGui.Services;

public static class EmbeddedResources
{
    // These names must match the EmbeddedResource LogicalName and AssemblyMetadata items in oxipng_GUI.csproj.
    public const string OxipngExecutable = "OxipngGui.oxipng.exe";
    public const string OxipngLicense = "OxipngGui.oxipng-LICENSE.txt";
    public const string AppLicense = "OxipngGui.LICENSE.txt";
    public const string ThirdPartyNotices = "OxipngGui.THIRD-PARTY-NOTICES.txt";
    public const string OxipngVersionKey = "OxipngVersion";

    private static readonly Assembly Assembly = typeof(EmbeddedResources).Assembly;

    public static Stream Open(string name) =>
        Assembly.GetManifestResourceStream(name)
        ?? throw new InvalidOperationException($"The embedded resource '{name}' is missing. Rebuild oxipng_GUI.");

    public static string ReadText(string name)
    {
        using var reader = new StreamReader(Open(name));
        return reader.ReadToEnd();
    }

    public static byte[] ReadBytes(string name)
    {
        using var stream = Open(name);
        using var buffer = new MemoryStream();
        stream.CopyTo(buffer);
        return buffer.ToArray();
    }

    public static string Metadata(string key) =>
        Assembly.GetCustomAttributes<AssemblyMetadataAttribute>()
            .FirstOrDefault(attribute => string.Equals(attribute.Key, key, StringComparison.Ordinal))?.Value
        ?? throw new InvalidOperationException($"The assembly metadata '{key}' is missing. Rebuild oxipng_GUI.");
}
