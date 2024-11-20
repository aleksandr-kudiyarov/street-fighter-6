namespace Kudiyarov.StreetFighter6.Common.Entities;

public record CharacterInfo
{
    public required string Name { get; init; }
    public required int Wins { get; init; }
    public required int Battles { get; init; }
}
