namespace Kudiyarov.StreetFighter6;

public record Configuration
{
    public required long ProfileId { get; init; }
    public required TimeSpan Delay { get; init; }
}