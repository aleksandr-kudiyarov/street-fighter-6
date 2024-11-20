using Kudiyarov.StreetFighter6.Common.Entities;
using Kudiyarov.StreetFighter6.Extensions;
using Kudiyarov.StreetFighter6.HttpDal;
using Spectre.Console;

var builder = Host.CreateApplicationBuilder(args);
builder.Logging.ClearProviders();
builder.AddStreetFighterClient();

var app = builder.Build();

var table = new Table();

await AnsiConsole.Live(table)
    .StartAsync(async ctx =>
    {
        var response = await GetResponse();
        table.InitTable(response);
        ctx.Refresh();

        while (true)
        {
            await Task.Delay(5_000);
            response = await GetResponse();
            table.RefreshTable(response);
            ctx.Refresh();
        }
    });

return;

async Task<GetWinRatesResponse> GetResponse()
{
    await using var serviceScope = app.Services.CreateAsyncScope();
    var client = serviceScope.ServiceProvider.GetRequiredService<StreetFighterClient>();
    var response = await client.GetResponse();
    return response;
}