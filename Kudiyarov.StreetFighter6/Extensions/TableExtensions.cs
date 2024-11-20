using Kudiyarov.StreetFighter6.Common.Entities;
using Spectre.Console;

namespace Kudiyarov.StreetFighter6.Extensions;

public static class TableExtensions
{
    public static void InitTable(this Table table, GetWinRatesResponse response)
    {
        table.AddColumn("Character");
        table.AddColumn("Wins");
        table.AddColumn("Battles");
        table.AddColumn("");

        foreach (var element in response.CharacterInfos)
        {
            var name = element.Name;
            var wins = element.Wins;
            var battles = element.Battles;
            var winsPercentage = (double)element.Wins / element.Battles;
                
            table.AddRow(
                name,
                wins.ToString(),
                battles.ToString(),
                winsPercentage.ToString("P1")
            );
        }
    }

    public static void RefreshTable(this Table table, GetWinRatesResponse response)
    {
        var row = 0;
            
        foreach (var character in response.CharacterInfos)
        {
            var wins = character.Wins;
            var battles = character.Battles;
            var winsPercentage = (double)character.Wins / character.Battles;
                
            table.UpdateCell(row, 1, wins.ToString());
            table.UpdateCell(row, 2, battles.ToString());
            table.UpdateCell(row, 3, winsPercentage.ToString("P1"));

            row++;
        }
    }
}
