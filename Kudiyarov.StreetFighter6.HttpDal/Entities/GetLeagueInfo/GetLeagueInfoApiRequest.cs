using System.Text.Json.Serialization;

namespace Kudiyarov.StreetFighter6.HttpDal.Entities.GetLeagueInfo;

public record GetLeagueInfoApiRequest
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