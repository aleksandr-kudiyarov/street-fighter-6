using Kudiyarov.StreetFighter6.Common.Entities;
using Kudiyarov.StreetFighter6.Extensions;
using Kudiyarov.StreetFighter6.HttpDal;
using Kudiyarov.StreetFighter6.Logic.Implementations;
using Kudiyarov.StreetFighter6.Logic.Interfaces;
using Spectre.Console;

namespace Kudiyarov.StreetFighter6;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        var configuration = builder.Configuration.GetRequiredValue<Configuration>("Configuration");
        var authOptions = builder.Configuration.GetRequiredValue<Authentication>("Authentication");
        
        builder.Logging.ClearProviders();
        builder.Services.AddStreetFighterClient(authOptions);
        builder.Services.AddSingleton<IStyleProvider, StyleProvider>();

        var app = builder.Build();
        var table = new Table();

        await AnsiConsole.Live(table)
            .StartAsync(async ctx =>
            {
                await InitTable(app.Services, table, ctx);

                while (true)
                {
                    await Task.Delay(configuration.Delay);
                    await RefreshTable(app.Services, table, ctx);
                }
            });
    }

    private static async Task RefreshTable(
        IServiceProvider serviceProvider,
        Table table,
        LiveDisplayContext ctx)
    {
        await using var serviceScope = serviceProvider.CreateAsyncScope();
        var innerServiceProvider = serviceScope.ServiceProvider;
        var client = innerServiceProvider.GetRequiredService<StreetFighterClient>();
        var styleProvider = innerServiceProvider.GetRequiredService<IStyleProvider>();

        var response = await GetResponse(client);
        table.RefreshTable(response, styleProvider);
        ctx.Refresh();
    }

    private static async Task InitTable(
        IServiceProvider serviceProvider,
        Table table,
        LiveDisplayContext ctx)
    {
        await using var serviceScope = serviceProvider.CreateAsyncScope();
        var innerServiceProvider = serviceScope.ServiceProvider;
        var client = innerServiceProvider.GetRequiredService<StreetFighterClient>();
        var styleProvider = innerServiceProvider.GetRequiredService<IStyleProvider>();
        
        var response = await GetResponse(client);
        table.InitTable(response, styleProvider);
        ctx.Refresh();
    }

    private static async Task<GetWinRatesResponse> GetResponse(StreetFighterClient client)
    {
        var response = await client.GetResponse();
        return response;
    }
}
