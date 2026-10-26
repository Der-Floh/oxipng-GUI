using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace OxipngGui.Services;

/// <summary>The result oxipng prints for a single file with <c>--json</c>.</summary>
public sealed class OxipngReport
{
    private const string SuccessStatus = "success";

    private OxipngReport(bool succeeded, string? error)
    {
        Succeeded = succeeded;
        Error = error;
    }

    public bool Succeeded { get; }

    public string? Error { get; }

    public static OxipngReport? Parse(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return null;

        try
        {
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            var report = new DataContractJsonSerializer(typeof(JsonReport)).ReadObject(stream) as JsonReport;
            var result = report?.Results?.FirstOrDefault();
            return result is null
                ? null
                : new OxipngReport(string.Equals(result.Status, SuccessStatus, StringComparison.Ordinal), result.Error);
        }
        catch (SerializationException)
        {
            return null;
        }
    }

    public override string ToString() => Succeeded ? "Success" : $"Error: {Error}";

    [DataContract]
    private sealed class JsonReport
    {
        [DataMember(Name = "results")]
        public JsonResult[]? Results { get; set; }
    }

    [DataContract]
    private sealed class JsonResult
    {
        [DataMember(Name = "status")]
        public string? Status { get; set; }

        [DataMember(Name = "error")]
        public string? Error { get; set; }
    }
}
