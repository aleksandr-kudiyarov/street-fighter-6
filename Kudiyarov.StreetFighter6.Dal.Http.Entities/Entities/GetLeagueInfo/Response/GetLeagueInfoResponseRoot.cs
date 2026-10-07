using System.Text.Json.Serialization;

namespace Kudiyarov.StreetFighter6.Dal.Http.Entities.Entities.GetLeagueInfo.Response;

public record GetLeagueInfoResponseRoot
{
    [JsonPropertyName("response")]
    public required GetLeagueInfoResponse Response { get; init; }
}