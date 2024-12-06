using System.Text.Json.Serialization;

namespace Kudiyarov.StreetFighter6.HttpDal.Entities.GetLeagueInfo.Response;

public record GetLeagueInfoResponse
{
    [JsonPropertyName("response")]
    public required Response Response { get; init; }
}