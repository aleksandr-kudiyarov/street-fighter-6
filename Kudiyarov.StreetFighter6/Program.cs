using Kudiyarov.StreetFighter6.Extensions;

var builder = Host.CreateApplicationBuilder(args);

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
    });

namespace Kudiyarov.StreetFighter6
{
    public static class TableExtensions
    {
        public static void InitTable(this Table table, GetWinRatesResponse response)
        {
            table.AddColumn("Character");
            table.AddColumn("Wins");
            table.AddColumn("Battles");
            table.AddColumn("");

            foreach (var element in response.CharacterInfos)
            {
                var name = element.Name;
                var wins = element.Wins;
                var battles = element.Battles;
                var winsPercentage = (double)element.Wins / element.Battles;
                
                table.AddRow(
                    name,
                    wins.ToString(),
                    battles.ToString(),
                    winsPercentage.ToString("P1")
                );
            }
        }
    }
}