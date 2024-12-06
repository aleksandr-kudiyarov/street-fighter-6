using System.Text.Json.Serialization;

namespace Kudiyarov.StreetFighter6.HttpDal.Entities.GetLeagueInfo.Response;

public record Response
{
    [JsonPropertyName("character_league_infos")]
    public required CharacterLeagueInfo[] CharacterLeagueInfos { get; init; }
}