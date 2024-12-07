namespace Kudiyarov.StreetFighter6.Common.Entities;

public record GetCharacterInfoResponse
{
    public required IEnumerable<CharacterInfo> CharacterInfos { get; init; }
}