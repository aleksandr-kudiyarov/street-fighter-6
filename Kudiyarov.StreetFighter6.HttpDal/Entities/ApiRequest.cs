using System.Text.Json.Serialization;

namespace Kudiyarov.StreetFighter6.HttpDal.Entities;

public abstract record ApiRequest
{
    [JsonPropertyName("targetShortId")]
    public required long TargetShortId { get; init; }
    
    [JsonPropertyName("targetSeasonId")]
    public required int TargetSeasonId { get; init; }
    
    [JsonPropertyName("locale")]
    public required string Locale { get; init; }
}
