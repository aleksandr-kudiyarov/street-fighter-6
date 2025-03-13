using System.Text.Json.Serialization;

namespace Kudiyarov.StreetFighter6.HttpDal.Entities.GetLeagueInfo;

public record GetLeagueInfoApiRequest : ApiRequest
{
    [JsonPropertyName("peak")]
    public required bool Peak { get; init; }
}