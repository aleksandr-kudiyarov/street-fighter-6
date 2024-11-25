using Kudiyarov.StreetFighter6.Common.Entities;
using Kudiyarov.StreetFighter6.HttpDal;
using Kudiyarov.StreetFighter6.Logic.Interfaces;
using Spectre.Console;

namespace Kudiyarov.StreetFighter6.TableWorkers;

public abstract class TableWorker
{
    public async Task Do(
        LiveDisplayContext ctx,
        Table table,
        IServiceProvider serviceProvider)
    {
        await using var serviceScope = serviceProvider.CreateAsyncScope();
        var innerServiceProvider = serviceScope.ServiceProvider;
        var client = innerServiceProvider.GetRequiredService<StreetFighterClient>();
        var styleProvider = innerServiceProvider.GetRequiredService<IStyleProvider>();

        var response = await client.GetResponse();
        Impl(table, response, styleProvider);
        ctx.Refresh();
    }

    protected abstract void Impl(
        Table table,
        GetWinRatesResponse response,
        IStyleProvider styleProvider);
}