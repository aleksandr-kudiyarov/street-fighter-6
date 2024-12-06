using Kudiyarov.StreetFighter6.HttpDal;
using Kudiyarov.StreetFighter6.HttpDal.Entities.GetWinRates.Response;
using Spectre.Console;

namespace Kudiyarov.StreetFighter6.TableWorkers;

public abstract class TableAction(StreetFighterClient client)
{
    public async Task Invoke(Table table)
    {
        var response = await client.GetWinRates();
        Action(table, response);
    }

    protected abstract void Action(Table table, Response response);

    protected IEnumerable<CharacterWinRates> GetCharacterWinRates(IEnumerable<CharacterWinRates> winRates)
    {
        return winRates.Where(IsCharacter);
    }
    
    private static bool IsCharacter(CharacterWinRates source)
    {
        const int any = 253;
        const int random = 254;
        var isCharacter = source.CharacterId is not (any or random);
        return isCharacter;
    }
}