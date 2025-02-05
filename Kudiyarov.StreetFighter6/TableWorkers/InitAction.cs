using Kudiyarov.StreetFighter6.Common.Entities;
using Kudiyarov.StreetFighter6.Logic;
using Kudiyarov.StreetFighter6.StyleProviders;
using Spectre.Console;

namespace Kudiyarov.StreetFighter6.TableWorkers;

public sealed class InitAction(
    StreetFighterLogic client,
    StyleProvider<LeagueEnum> leagueStyleProvider,
    StyleProvider<Percentage> percentageStyleProvider)
    : TableAction(
        client,
        leagueStyleProvider,
        percentageStyleProvider)
{
    protected override void Action(Table table, GetCharacterInfoResponse response)
    {
        table.AddColumn("Character");
        table.AddColumn("Wins");
        table.AddColumn("Battles");
        table.AddColumn("Wins %");
        table.AddColumn("League Points");
        table.AddColumn("League");

        foreach (var character in GetCharacterWinRates(response.CharacterInfos))
        {
            var name = character.CharacterName;

            table.AddRow(
                new Text(name),
                GetWins(character),
                GetBattles(character),
                GetWinsPercentage(character),
                GetLeaguePoints(character),
                GetLeagueLevel(character)
            );
        }
    }
}