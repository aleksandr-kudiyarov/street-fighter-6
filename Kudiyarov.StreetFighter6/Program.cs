using Kudiyarov.StreetFighter6.Extensions;
using Kudiyarov.StreetFighter6.HttpDal;
using Spectre.Console;

var builder = Host.CreateApplicationBuilder(args);
builder.Logging.ClearProviders();
builder.AddStreetFighterClient();

var app = builder.Build();

var client = app.Services.GetRequiredService<StreetFighterClient>();

var table = new Table();

await AnsiConsole.Live(table)
    .StartAsync(async ctx =>
    {
        var response = await client.GetResponse();
        table.InitTable(response);
        ctx.Refresh();

        while (true)
        {
            await Task.Delay(5_000);
            response = await client.GetResponse();
            table.RefreshTable(response);
            ctx.Refresh();
        }
    });