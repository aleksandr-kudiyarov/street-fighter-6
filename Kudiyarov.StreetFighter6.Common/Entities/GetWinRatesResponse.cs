namespace Kudiyarov.StreetFighter6.Common.Entities;

public record GetWinRatesResponse
{
    public required IEnumerable<CharacterInfo> CharacterInfos { get; init; }
}