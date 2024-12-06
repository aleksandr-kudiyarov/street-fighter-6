using System.Text.Json.Serialization;

namespace Kudiyarov.StreetFighter6.HttpDal.Entities.GetLeagueInfo.Response;

public record LeagueInfo
{
    [JsonPropertyName("league_point")]
    public required int LeaguePoint { get; init; }
    
    [JsonPropertyName("league_rank")]
    public required int LeagueRank { get; init; }
    
    [JsonPropertyName("master_league")]
    public required int MasterLeague { get; init; }
    
    [JsonPropertyName("master_rating")]
    public required int MasterRating { get; init; }
    
    [JsonPropertyName("master_rating_ranking")]
    public required int MasterRatingRanking { get; init; }
}