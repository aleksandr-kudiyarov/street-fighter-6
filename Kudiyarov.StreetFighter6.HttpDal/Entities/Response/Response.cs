using System.Text.Json.Serialization;

namespace Kudiyarov.StreetFighter6.HttpDal.Entities.Response;

public record Response
{
    [JsonPropertyName("character_win_rates")]
    public required CharacterWinRates[] CharacterWinRate { get; init; }
}