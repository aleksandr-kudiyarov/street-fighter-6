using System.Text.Json.Serialization;

namespace Kudiyarov.StreetFighter6.HttpDal.Entities.Response;

public record RootResponse
{
    [JsonPropertyName("response")]
    public required Response Response { get; init; }
}