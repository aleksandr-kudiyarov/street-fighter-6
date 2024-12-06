using System.Text.Json.Serialization;

namespace Kudiyarov.StreetFighter6.HttpDal.Entities.GetLeagueInfo.Response;

public record CharacterLeagueInfo
{
    [JsonPropertyName("character_id")]
    public required int CharacterId { get; init; }
    
    [JsonPropertyName("is_played")]
    public required bool IsPlayed { get; init; }
    
    [JsonPropertyName("league_info")]
    public required LeagueInfo LeagueInfo { get; init; }
    
    [JsonPropertyName("character_name")]
    public required string CharacterName { get; init; }
    
    [JsonPropertyName("character_alpha")]
    public required string CharacterAlpha { get; init; }
    
    [JsonPropertyName("character_tool_name")]
    public required string CharacterToolName { get; init; }
    
    [JsonPropertyName("character_sort")]
    public required int CharacterSort { get; init; }
}