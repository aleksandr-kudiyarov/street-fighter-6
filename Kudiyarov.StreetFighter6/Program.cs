using Kudiyarov.StreetFighter6.Common.Entities;
using Kudiyarov.StreetFighter6.Extensions;
using Kudiyarov.StreetFighter6.Logic;
using Kudiyarov.StreetFighter6.StyleProviders;
using Kudiyarov.StreetFighter6.TableWorkers;
using Spectre.Console;

namespace Kudiyarov.StreetFighter6;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        var configuration = builder.Configuration.GetRequiredValue<Configuration>("Configuration");
        var authentication = builder.Configuration.GetRequiredValue<Authentication>("Authentication");
        
        builder.Logging.ClearProviders();
        builder.Services.AddScoped<StreetFighterLogic>();
        builder.Services.AddScoped<InitAction>();
        builder.Services.AddScoped<UpdateAction>();
        builder.Services.AddStreetFighterClient(authentication);
        builder.Services.AddSingleton<StyleProvider<Percentage>, WinRateStyleProvider>();
        builder.Services.AddSingleton<StyleProvider<LeagueEnum>, LeagueInfoStyleProvider>();
        builder.Services.AddMemoryCache();

        var app = builder.Build();
        var table = new Table();
        var request = new GetCharacterInfosRequest(configuration.ProfileId);

        await AnsiConsole.Live(table)
            .StartAsync(async ctx =>
            {
                await InvokeTableAction<InitAction>(request, ctx, table, app.Services);

                while (true)
                {
                    await Task.Delay(configuration.Delay);
                    await InvokeTableAction<UpdateAction>(request, ctx, table, app.Services);
                }
            });
    }
 
    private static async Task InvokeTableAction<TAction>(
        GetCharacterInfosRequest request,
        LiveDisplayContext ctx,
        Table table,
        IServiceProvider serviceProvider)
        where TAction : TableAction
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var action = scope.ServiceProvider.GetRequiredService<TAction>();
        await action.Invoke(request, table);
        ctx.Refresh();
    }
}