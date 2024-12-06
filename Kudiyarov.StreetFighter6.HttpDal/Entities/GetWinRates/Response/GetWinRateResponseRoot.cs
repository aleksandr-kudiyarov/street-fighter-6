using System.Text.Json.Serialization;

namespace Kudiyarov.StreetFighter6.HttpDal.Entities.GetWinRates.Response;

public record GetWinRateResponseRoot
{
    [JsonPropertyName("response")]
    public required GetWinRateResponse Response { get; init; }
}