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
        builder.Services.AddScoped<InitAction>();
        builder.Services.AddScoped<UpdateAction>();
        builder.Services.AddStreetFighterClient(authOptions);
        builder.Services.AddSingleton<IStyleProvider, StyleProvider>();

        var app = builder.Build();
        var table = new Table();

        await AnsiConsole.Live(table)
            .StartAsync(async ctx =>
            {
                await InvokeTableAction<InitAction>(ctx, table, app.Services);

                while (true)
                {
                    await Task.Delay(configuration.Delay);
                    await InvokeTableAction<UpdateAction>(ctx, table, app.Services);
                }
            });
    }
 
    private static async Task InvokeTableAction<TAction>(
        LiveDisplayContext ctx,
        Table table,
        IServiceProvider serviceProvider)
        where TAction : TableAction
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var action = scope.ServiceProvider.GetRequiredService<TAction>();
        await action.Invoke(table);
        ctx.Refresh();
    }
}