namespace Kudiyarov.StreetFighter6.Common;

public record GetCharacterInfoResponse
{
    public required IEnumerable<CharacterInfo> CharacterInfos { get; init; }
}