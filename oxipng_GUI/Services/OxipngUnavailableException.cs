namespace OxipngGui.Services;

public sealed class OxipngUnavailableException(string message, bool isRuntimeMissing, Exception? innerException = null)
    : Exception(message, innerException)
{
    public bool IsRuntimeMissing { get; } = isRuntimeMissing;
}
