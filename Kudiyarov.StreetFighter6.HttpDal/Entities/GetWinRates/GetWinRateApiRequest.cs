using System.Text.Json.Serialization;

namespace Kudiyarov.StreetFighter6.HttpDal.Entities.GetWinRates;

public record GetWinRateApiRequest : ApiRequest
{
    [JsonPropertyName("targetModeId")]
    public required int TargetModeId { get; init; }
}