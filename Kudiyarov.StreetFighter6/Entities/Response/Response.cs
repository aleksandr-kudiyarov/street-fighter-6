using System.Text.Json.Serialization;

namespace Kudiyarov.StreetFighter6.Entities.Response;

public record Response
{
    [JsonPropertyName("character_win_rates")]
    public required CharacterWinRates[] CharacterWinRates { get; init; }
}