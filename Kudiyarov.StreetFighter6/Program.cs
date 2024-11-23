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
        var configuration = GetConfiguration(builder.Configuration);
        builder.Logging.ClearProviders();
        builder.AddStreetFighterClient();
        builder.Services.AddSingleton<IStyleProvider, StyleProvider>();

        var app = builder.Build();
        var table = new Table();

        await AnsiConsole.Live(table)
            .StartAsync(async ctx =>
            {
                await InitTable(app, table, ctx);

                while (true)
                {
                    await Task.Delay(configuration.Delay);
                    await RefreshTable(app, table, ctx);
                }
            });
    }

    private static async Task RefreshTable(IHost app, Table table, LiveDisplayContext ctx)
    {
        await using var serviceScope = app.Services.CreateAsyncScope();
        var serviceProvider = serviceScope.ServiceProvider;
        var client = serviceProvider.GetRequiredService<StreetFighterClient>();
        var styleProvider = serviceProvider.GetRequiredService<IStyleProvider>();

        var response = await GetResponse(client);
        table.RefreshTable(response, styleProvider);
        ctx.Refresh();
    }

    private static async Task InitTable(IHost app, Table table, LiveDisplayContext ctx)
    {
        await using var serviceScope = app.Services.CreateAsyncScope();
        var serviceProvider = serviceScope.ServiceProvider;
        var client = serviceProvider.GetRequiredService<StreetFighterClient>();
        var styleProvider = serviceProvider.GetRequiredService<IStyleProvider>();
        
        var response = await GetResponse(client);
        table.InitTable(response, styleProvider);
        ctx.Refresh();
    }

    private static async Task<GetWinRatesResponse> GetResponse(StreetFighterClient client)
    {
        var response = await client.GetResponse();
        return response;
    }
    
    private static Configuration GetConfiguration(IConfiguration configuration)
    {
        var config = configuration
            .GetSection("Configuration")
            .Get<Configuration>();

        ArgumentNullException.ThrowIfNull(config);
    
        return config;
    }
}
