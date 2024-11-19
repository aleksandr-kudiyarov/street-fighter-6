namespace Kudiyarov.StreetFighter6.Common.Entities;

public record WinRate
{
    public required string CharacterName { get; init; }
    public required int WinsCount { get; init; }
    public required int BattlesCount { get; init; }
    
    public double WinsPercentage => (double)WinsCount / BattlesCount;
}