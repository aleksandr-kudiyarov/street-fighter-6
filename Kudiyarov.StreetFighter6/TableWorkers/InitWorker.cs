using Kudiyarov.StreetFighter6.Common.Entities;
using Kudiyarov.StreetFighter6.Logic.Interfaces;
using Spectre.Console;

namespace Kudiyarov.StreetFighter6.TableWorkers;

public sealed class InitWorker : TableWorker
{
    protected override void Impl(
        Table table,
        GetWinRatesResponse response,
        IStyleProvider styleProvider)
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