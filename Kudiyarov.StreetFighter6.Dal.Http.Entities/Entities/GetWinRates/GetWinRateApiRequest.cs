using System.Text.Json.Serialization;

namespace Kudiyarov.StreetFighter6.Dal.Http.Entities.Entities.GetWinRates;

public record GetWinRateApiRequest : ApiRequest
{
    [JsonPropertyName("targetModeId")]
    public required int TargetModeId { get; init; }
}