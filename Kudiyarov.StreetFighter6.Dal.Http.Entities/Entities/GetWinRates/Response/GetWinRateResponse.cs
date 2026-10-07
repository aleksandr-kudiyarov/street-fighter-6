using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Kudiyarov.StreetFighter6.Dal.Http.Entities.Entities.GetWinRates.Response;

[ImmutableObject(true)]
public sealed record GetWinRateResponse
{
    [JsonPropertyName("character_win_rates")]
    public required IReadOnlyList<CharacterWinRates> CharacterWinRate { get; init; }
}