using Kudiyarov.StreetFighter6.HttpDal;
using Kudiyarov.StreetFighter6.HttpDal.Entities.GetWinRates.Response;
using Kudiyarov.StreetFighter6.Logic.Interfaces;
using Spectre.Console;

namespace Kudiyarov.StreetFighter6.TableWorkers;

public sealed class UpdateAction(
    StreetFighterClient client,
    IStyleProvider styleProvider)
    : TableAction(client)
{
    protected override void Action(Table table, GetWinRateResponse response)
    {
        var row = 0;
            
        foreach (var character in GetCharacterWinRates(response.CharacterWinRate))
        {
            var wins = character.WinCount;
            var battles = character.BattleCount;
            var winsPercentage = (double)character.WinCount / character.BattleCount;
            var winsPercentageStyle = styleProvider.GetStyle(winsPercentage);
                
            table.UpdateCell(row, 1, new Text(wins.ToString()));
            table.UpdateCell(row, 2, new Text(battles.ToString()));
            table.UpdateCell(row, 3, new Text(winsPercentage.ToString("P1"), winsPercentageStyle));

            row++;
        }
    }
}