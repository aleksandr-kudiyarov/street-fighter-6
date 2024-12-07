using Kudiyarov.StreetFighter6.Common.Entities;
using Kudiyarov.StreetFighter6.Logic;
using Spectre.Console;

namespace Kudiyarov.StreetFighter6.TableWorkers;

public sealed class UpdateAction(
    StreetFighterLogic client,
    StyleProvider<Percentage> percentageStyleProvider,
    StyleProvider<LeagueEnum> leagueStyleProvider)
    : TableAction(client)
{
    protected override void Action(Table table, GetCharacterInfoResponse response)
    {
        var row = 0;

        foreach (var character in GetCharacterWinRates(response.CharacterInfos))
        {
            var wins = character.WinCount;
            var battles = character.BattleCount;
            var winsPercentage = (double)character.WinCount / character.BattleCount;
            var winsPercentageStyle = percentageStyleProvider.GetStyle(winsPercentage);
            var leaguePoints = character.LeaguePoint;

            var leagueInfo = GetLeagueInfo(leaguePoints);
            var leagueLevel = GetLeagueLevel(leagueInfo.Level);
            var leagueStyle = leagueStyleProvider.GetStyle(leagueInfo.League);

            table.UpdateCell(row, 1, new Text(wins.ToString()));
            table.UpdateCell(row, 2, new Text(battles.ToString()));
            table.UpdateCell(row, 3, new Text(winsPercentage.ToString("P1"), winsPercentageStyle));
            table.UpdateCell(row, 4, new Text(leaguePoints.ToString()));
            table.UpdateCell(row, 5, new Text(leagueLevel, leagueStyle));

            row++;
        }
    }
}