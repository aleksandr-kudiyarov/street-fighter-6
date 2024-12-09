namespace Kudiyarov.StreetFighter6.Common.Entities;

public record GetCharacterInfoRequest
{
    public required long ProfileId { get; init; }
    public required int Season { get; init; }
}