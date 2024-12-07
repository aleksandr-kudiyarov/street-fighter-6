using Kudiyarov.StreetFighter6.Common;
using Kudiyarov.StreetFighter6.Logic.Implementations;
using Spectre.Console;

namespace Kudiyarov.StreetFighter6.TableWorkers;

public abstract class TableAction(StreetFighterLogic client)
{
    public async Task Invoke(Table table)
    {
        var response = await client.GetCharacterInfos();
        Action(table, response);
    }

    protected abstract void Action(Table table, GetCharacterInfoResponse response);

    protected static IEnumerable<CharacterInfo> GetCharacterWinRates(IEnumerable<CharacterInfo> winRates)
    {
        return winRates.Where(IsCharacter);
    }
    
    private static bool IsCharacter(CharacterInfo source)
    {
        const int any = 253;
        const int random = 254;
        var isCharacter = source.CharacterId is not (any or random);
        return isCharacter;
    }
}