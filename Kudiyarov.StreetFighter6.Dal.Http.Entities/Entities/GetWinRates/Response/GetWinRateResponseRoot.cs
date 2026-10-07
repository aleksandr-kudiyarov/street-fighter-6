using System.Text.Json.Serialization;

namespace Kudiyarov.StreetFighter6.Dal.Http.Entities.Entities.GetWinRates.Response;

public record GetWinRateResponseRoot
{
    [JsonPropertyName("response")]
    public required GetWinRateResponse Response { get; init; }
}