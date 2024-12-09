namespace Kudiyarov.StreetFighter6.Common.Entities;

public record GetLeagueInfoRequest
{
    public required long ProfileId { get; init; }
    public required int SeasonId { get; init; }
}