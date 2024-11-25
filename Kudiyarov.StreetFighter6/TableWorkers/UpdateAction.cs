using Kudiyarov.StreetFighter6.Common.Entities;
using Kudiyarov.StreetFighter6.HttpDal;
using Kudiyarov.StreetFighter6.Logic.Interfaces;
using Spectre.Console;

namespace Kudiyarov.StreetFighter6.TableWorkers;

public sealed class UpdateAction(
    StreetFighterClient client,
    IStyleProvider styleProvider)
    : TableAction(client)
{
    protected override void Action(Table table, GetWinRatesResponse response)
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