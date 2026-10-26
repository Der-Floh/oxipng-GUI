namespace OxipngGui.Utils;

public static class ByteSize
{
    private const double UnitStep = 1024;
    private static readonly string[] Units = ["B", "KB", "MB", "GB", "TB"];

    public static string Format(long bytes)
    {
        var value = (double)bytes;
        var unit = 0;
        while (Math.Abs(value) >= UnitStep && unit < Units.Length - 1)
        {
            value /= UnitStep;
            unit++;
        }

        return unit == 0 ? $"{bytes} {Units[0]}" : $"{value:0.0} {Units[unit]}";
    }
}
