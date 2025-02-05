namespace Kudiyarov.StreetFighter6.Common.Entities;

public record CharacterInfo
{
    public required int CharacterId { get; init; }
    public required string CharacterName { get; init; }
    public required int CharacterSort { get; init; }
    
    public required int WinCount { get; init; }
    public required int BattleCount { get; init; }
    
    public required int? LeaguePoint { get; init; }
}
