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
                var response = await GetResponse(app);
                table.InitTable(response);
                ctx.Refresh();

                while (true)
                {
                    await Task.Delay(configuration.Delay);
                    response = await GetResponse(app);
                    table.RefreshTable(response);
                    ctx.Refresh();
                }
            });
    }

    private static async Task<GetWinRatesResponse> GetResponse(IHost app)
    {
        await using var serviceScope = app.Services.CreateAsyncScope();
        var client = serviceScope.ServiceProvider.GetRequiredService<StreetFighterClient>();
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
