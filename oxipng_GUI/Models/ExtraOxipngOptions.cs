namespace OxipngGui.Models;

/// <summary>oxipng options from the command line that the window has no control for.</summary>
/// <param name="arguments">Passed to oxipng after the window's own flags.</param>
/// <param name="controlsMetadata">
/// <c>--keep</c> or <c>--strip</c> with a chunk list decides the metadata, so the Metadata control mustn't add a flag.
/// </param>
/// <param name="requiredZopfli">
/// <see langword="true"/> for <c>--zi</c>/<c>--ziwi</c>, which need Zopfli; <see langword="false"/> for <c>--zc</c>,
/// which rules it out; <see langword="null"/> if Zopfli is up to the window.
/// </param>
public sealed class ExtraOxipngOptions(IReadOnlyList<string> arguments, bool controlsMetadata, bool? requiredZopfli)
{
    public static ExtraOxipngOptions None { get; } = new([], false, null);

    public IReadOnlyList<string> Arguments { get; } = arguments;

    public bool ControlsMetadata { get; } = controlsMetadata;

    public bool? RequiredZopfli { get; } = requiredZopfli;

    public override string ToString() => string.Join(" ", Arguments);
}
