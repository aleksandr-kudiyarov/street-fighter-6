using Kudiyarov.StreetFighter6.HttpDal;
using Kudiyarov.StreetFighter6.HttpDal.Entities.GetWinRates.Response;
using Kudiyarov.StreetFighter6.Logic.Interfaces;
using Spectre.Console;

namespace Kudiyarov.StreetFighter6.TableWorkers;

public sealed class InitAction(
    StreetFighterClient client,
    IStyleProvider styleProvider)
    : TableAction(client)
{
    protected override void Action(Table table, Response response)
    {
        table.AddColumn("Character");
        table.AddColumn("Wins");
        table.AddColumn("Battles");
        table.AddColumn("Wins %");

        foreach (var element in GetCharacterWinRates(response.CharacterWinRate))
        {
            var name = element.CharacterName;
            var wins = element.WinCount;
            var battles = element.BattleCount;
            var winsPercentage = (double)element.WinCount / element.BattleCount;
            var winsPercentageStyle = styleProvider.GetStyle(winsPercentage);
                
            table.AddRow(
                new Text(name),
                new Text(wins.ToString()),
                new Text(battles.ToString()),
                new Text(winsPercentage.ToString("P1"), winsPercentageStyle)
            );
        }
    }
}