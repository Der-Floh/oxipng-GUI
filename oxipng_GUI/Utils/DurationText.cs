namespace OxipngGui.Utils;

public static class DurationText
{
    private const int SecondsStep = 10;
    private const int StepsPerMinute = 60 / SecondsStep;
    private const int MinutesPerHour = 60;

    /// <summary>A rounded duration such as "about 40 s", "about 4 min" or "about 1 h 20 min".</summary>
    public static string Approximate(TimeSpan duration)
    {
        var steps = (int)Math.Round(duration.TotalSeconds / SecondsStep);
        if (steps < 1)
            return "a few seconds";
        if (steps < StepsPerMinute)
            return $"about {steps * SecondsStep} s";

        var minutes = (int)Math.Round(duration.TotalMinutes);
        if (minutes < MinutesPerHour)
            return $"about {minutes} min";

        var hours = minutes / MinutesPerHour;
        var restMinutes = minutes % MinutesPerHour;
        return restMinutes == 0 ? $"about {hours} h" : $"about {hours} h {restMinutes} min";
    }

    /// <summary>A duration as a clock reading, such as "0:07", "12:34" or "1:02:03".</summary>
    public static string Clock(TimeSpan duration)
    {
        var hours = (int)duration.TotalHours;
        return hours > 0
            ? $"{hours}:{duration.Minutes:00}:{duration.Seconds:00}"
            : $"{duration.Minutes}:{duration.Seconds:00}";
    }
}
