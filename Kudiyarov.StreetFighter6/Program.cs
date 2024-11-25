using Kudiyarov.StreetFighter6.Extensions;
using Kudiyarov.StreetFighter6.Logic.Implementations;
using Kudiyarov.StreetFighter6.Logic.Interfaces;
using Kudiyarov.StreetFighter6.TableWorkers;
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

        builder.Services.AddSingleton<InitWorker>();
        builder.Services.AddSingleton<UpdateWorker>();

        var app = builder.Build();
        var table = new Table();
        
        var initWorker = app.Services.GetRequiredService<InitWorker>();
        var updateWorker = app.Services.GetRequiredService<UpdateWorker>();

        await AnsiConsole.Live(table)
            .StartAsync(async ctx =>
            {
                await initWorker.Do(ctx, table, app.Services);

                while (true)
                {
                    await Task.Delay(configuration.Delay);
                    await updateWorker.Do(ctx, table, app.Services);
                }
            });
    }
}