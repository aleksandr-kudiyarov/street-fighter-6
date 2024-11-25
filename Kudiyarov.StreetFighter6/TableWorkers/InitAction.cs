using Kudiyarov.StreetFighter6.Common.Entities;
using Kudiyarov.StreetFighter6.HttpDal;
using Kudiyarov.StreetFighter6.Logic.Interfaces;
using Spectre.Console;

namespace Kudiyarov.StreetFighter6.TableWorkers;

public sealed class InitAction(
    StreetFighterClient client,
    IStyleProvider styleProvider)
    : TableAction(client)
{
    protected override void Action(Table table, GetWinRatesResponse response)
    {
        table.AddColumn("Character");
        table.AddColumn("Wins");
        table.AddColumn("Battles");
        table.AddColumn("Wins %");

        foreach (var element in response.CharacterInfos)
        {
            var name = element.Name;
            var wins = element.Wins;
            var battles = element.Battles;
            var winsPercentage = (double)element.Wins / element.Battles;
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