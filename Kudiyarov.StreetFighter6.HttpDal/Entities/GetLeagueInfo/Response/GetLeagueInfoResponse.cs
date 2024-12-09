using System.Text.Json.Serialization;

namespace Kudiyarov.StreetFighter6.HttpDal.Entities.GetLeagueInfo.Response;

public record GetLeagueInfoResponse
{
    [JsonPropertyName("character_league_infos")]
    public required IReadOnlyList<CharacterLeagueInfo> CharacterLeagueInfos { get; init; }
}