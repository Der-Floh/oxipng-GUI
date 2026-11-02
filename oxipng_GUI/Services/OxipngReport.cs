using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace OxipngGui.Services;

/// <summary>The result oxipng prints for one file with <c>--json</c>.</summary>
public sealed class OxipngReport
{
    private const string SuccessStatus = "success";

    private OxipngReport(string input, bool succeeded, string? error)
    {
        Input = input;
        Succeeded = succeeded;
        Error = error;
    }

    /// <summary>The file's path, as oxipng read it.</summary>
    public string Input { get; }

    public bool Succeeded { get; }

    public string? Error { get; }

    /// <returns>The result for each file, or <see langword="null"/> if <paramref name="json"/> isn't oxipng's report.</returns>
    public static IReadOnlyList<OxipngReport>? Parse(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return null;

        try
        {
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            var report = new DataContractJsonSerializer(typeof(JsonReport)).ReadObject(stream) as JsonReport;
            return report?.Results?.Select(ToReport).ToList();
        }
        catch (SerializationException)
        {
            return null;
        }
    }

    public override string ToString() => Succeeded ? $"{Input}: success" : $"{Input}: error {Error}";

    private static OxipngReport ToReport(JsonResult result) =>
        new(result.Input ?? string.Empty, string.Equals(result.Status, SuccessStatus, StringComparison.Ordinal), result.Error);

    [DataContract]
    private sealed class JsonReport
    {
        [DataMember(Name = "results")]
        public JsonResult[]? Results { get; set; }
    }

    [DataContract]
    private sealed class JsonResult
    {
        [DataMember(Name = "input")]
        public string? Input { get; set; }

        [DataMember(Name = "status")]
        public string? Status { get; set; }

        [DataMember(Name = "error")]
        public string? Error { get; set; }
    }
}
