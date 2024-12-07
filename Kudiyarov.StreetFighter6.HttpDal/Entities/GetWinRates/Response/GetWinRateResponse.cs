using System.Text.Json.Serialization;

namespace Kudiyarov.StreetFighter6.HttpDal.Entities.GetWinRates.Response;

public record GetWinRateResponse
{
    [JsonPropertyName("character_win_rates")]
    public required CharacterWinRates[] CharacterWinRate { get; init; }
}