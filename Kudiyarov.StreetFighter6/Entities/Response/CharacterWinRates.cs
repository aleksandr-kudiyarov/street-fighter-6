using System.Text.Json.Serialization;

namespace Kudiyarov.StreetFighter6.Entities.Response;

public record CharacterWinRates
{
    [JsonPropertyName("battle_count")]
    public required int BattleCount { get; init; }
    
    [JsonPropertyName("character_id")]
    public required int CharacterId { get; init; }
    
    [JsonPropertyName("win_count")]
    public required int WinCount { get; init; }
    
    [JsonPropertyName("character_name")]
    public required string CharacterName { get; init; }
    
    [JsonPropertyName("character_alpha")]
    public required string CharacterAlpha { get; init; }
    
    [JsonPropertyName("character_tool_name")]
    public required string CharacterToolName { get; init; }
    
    [JsonPropertyName("character_sort")]
    public required int CharacterSort { get; init; }
    
    [JsonPropertyName("charac_ter_id")]
    public required int CharacTerId { get; init; }
    
    [JsonPropertyName("batt_le_count")]
    public required int BattLeCount { get; init; }
    
    [JsonPropertyName("charac_ter_sort")]
    public required int CharacTerSort { get; init; }
}