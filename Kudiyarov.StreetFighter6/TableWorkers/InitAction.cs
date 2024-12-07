using Kudiyarov.StreetFighter6.Common.Entities;
using Kudiyarov.StreetFighter6.Logic.Implementations;
using Kudiyarov.StreetFighter6.Logic.Interfaces;
using Spectre.Console;

namespace Kudiyarov.StreetFighter6.TableWorkers;

public sealed class InitAction(
    StreetFighterLogic client,
    IStyleProvider styleProvider)
    : TableAction(client)
{
    protected override void Action(Table table, GetCharacterInfoResponse response)
    {
        table.AddColumn("Character");
        table.AddColumn("Wins");
        table.AddColumn("Battles");
        table.AddColumn("Wins %");
        table.AddColumn("League Points");
        table.AddColumn("League");

        foreach (var element in GetCharacterWinRates(response.CharacterInfos))
        {
            var name = element.CharacterName;
            var wins = element.WinCount;
            var battles = element.BattleCount;
            var winsPercentage = (double)element.WinCount / element.BattleCount;
            var winsPercentageStyle = styleProvider.GetWinRateStyle(winsPercentage);
            var leaguePoints = element.LeaguePoint;
            
            var leagueInfo = GetLeagueInfo(leaguePoints);
            var leagueLevel = GetLeagueLevel(leagueInfo.Level);
            var leagueStyle = styleProvider.GetLeagueStyle(leagueInfo.League);
            
            table.AddRow(
                new Text(name),
                new Text(wins.ToString()),
                new Text(battles.ToString()),
                new Text(winsPercentage.ToString("P1"), winsPercentageStyle),
                new Text(leaguePoints.ToString()),
                new Text(leagueLevel, leagueStyle)
            );
        }
    }
}