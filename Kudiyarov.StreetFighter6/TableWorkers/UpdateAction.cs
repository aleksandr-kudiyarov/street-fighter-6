using Kudiyarov.StreetFighter6.Common;
using Kudiyarov.StreetFighter6.Logic.Implementations;
using Kudiyarov.StreetFighter6.Logic.Interfaces;
using Spectre.Console;

namespace Kudiyarov.StreetFighter6.TableWorkers;

public sealed class UpdateAction(
    StreetFighterLogic client,
    IStyleProvider styleProvider)
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
            var winsPercentageStyle = styleProvider.GetStyle(winsPercentage);
                
            table.UpdateCell(row, 1, new Text(wins.ToString()));
            table.UpdateCell(row, 2, new Text(battles.ToString()));
            table.UpdateCell(row, 3, new Text(winsPercentage.ToString("P1"), winsPercentageStyle));

            row++;
        }
    }
}