using System.Text.Json.Serialization;

namespace Kudiyarov.StreetFighter6.HttpDal.Entities.GetWinRates.Request;

public record RootRequest
{
    [JsonPropertyName("targetShortId")]
    public required long TargetShortId { get; init; }
    
    [JsonPropertyName("targetSeasonId")]
    public required int TargetSeasonId { get; init; }
    
    [JsonPropertyName("targetModeId")]
    public required int TargetModeId { get; init; }
    
    [JsonPropertyName("locale")]
    public required string Locale { get; init; }
}
