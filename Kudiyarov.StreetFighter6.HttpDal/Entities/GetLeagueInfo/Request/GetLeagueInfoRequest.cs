using System.Text.Json.Serialization;

namespace Kudiyarov.StreetFighter6.HttpDal.Entities.GetLeagueInfo.Request;

public record GetLeagueInfoRequest
{
    [JsonPropertyName("targetShortId")]
    public required long TargetShortId { get; init; }
    
    [JsonPropertyName("targetSeasonId")]
    public required long TargetSeasonId { get; init; }
    
    [JsonPropertyName("locale")]
    public required string Locale { get; init; }
    
    [JsonPropertyName("peak")]
    public required bool Peak { get; init; }
}