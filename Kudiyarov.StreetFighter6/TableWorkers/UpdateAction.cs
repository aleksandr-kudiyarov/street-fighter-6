using Kudiyarov.StreetFighter6.Common.Entities;
using Kudiyarov.StreetFighter6.Logic;
using Kudiyarov.StreetFighter6.StyleProviders;
using Spectre.Console;

namespace Kudiyarov.StreetFighter6.TableWorkers;

public sealed class UpdateAction(
    StreetFighterLogic client,
    StyleProvider<LeagueEnum> leagueStyleProvider,
    StyleProvider<Percentage> percentageStyleProvider)
    : TableAction(
        client,
        leagueStyleProvider,
        percentageStyleProvider)
{
    protected override void Action(Table table, GetCharacterInfoResponse response)
    {
        foreach (var (row, character) in GetCharacterWinRates(response.CharacterInfos).Index())
        {
            table.UpdateCell(row, 1, GetWins(character));
            table.UpdateCell(row, 2, GetBattles(character));
            table.UpdateCell(row, 3, GetWinsPercentage(character));
            table.UpdateCell(row, 4, GetLeaguePoints(character));
            table.UpdateCell(row, 5, GetLeagueLevel(character));
        }
    }
}