using Kudiyarov.StreetFighter6.Common.Entities;
using Kudiyarov.StreetFighter6.Logic.Interfaces;
using Spectre.Console;

namespace Kudiyarov.StreetFighter6.Extensions;

public static class TableExtensions
{
    public static void InitTable(
        this Table table,
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

    public static void RefreshTable(
        this Table table,
        GetWinRatesResponse response,
        IStyleProvider styleProvider)
    {
        var row = 0;
            
        foreach (var character in response.CharacterInfos)
        {
            var wins = character.Wins;
            var battles = character.Battles;
            var winsPercentage = (double)character.Wins / character.Battles;
            var winsPercentageStyle = styleProvider.GetStyle(winsPercentage);
                
            table.UpdateCell(row, 1, new Text(wins.ToString()));
            table.UpdateCell(row, 2, new Text(battles.ToString()));
            table.UpdateCell(row, 3, new Text(winsPercentage.ToString("P1"), winsPercentageStyle));

            row++;
        }
    }
}
