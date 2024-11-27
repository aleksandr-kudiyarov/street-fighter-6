using Kudiyarov.StreetFighter6.Common.Entities;
using Kudiyarov.StreetFighter6.HttpDal;
using Spectre.Console;

namespace Kudiyarov.StreetFighter6.TableWorkers;

public abstract class TableAction(StreetFighterClient client)
{
    public async Task Invoke(Table table)
    {
        var response = await client.GetResponse();
        Action(table, response);
    }

    protected abstract void Action(Table table, GetWinRatesResponse response);
}