using System.Text.Json.Serialization;

namespace Kudiyarov.StreetFighter6.Entities.Response;

public record RootResponse
{
    [JsonPropertyName("response")]
    public required Response Response { get; init; }
}