using Kudiyarov.StreetFighter6.Common.Entities;
using Kudiyarov.StreetFighter6.HttpDal;
using Spectre.Console;

namespace Kudiyarov.StreetFighter6.TableWorkers;

public abstract class TableAction(StreetFighterClient client)
{
    public async Task Invoke(
        LiveDisplayContext ctx,
        Table table)
    {
        var response = await client.GetResponse();
        Action(table, response);
        ctx.Refresh();
    }

    protected abstract void Action(Table table, GetWinRatesResponse response);
}