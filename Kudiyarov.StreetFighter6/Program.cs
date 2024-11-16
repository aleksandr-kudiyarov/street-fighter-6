using Kudiyarov.StreetFighter6.Extensions;

var builder = Host.CreateApplicationBuilder(args);

builder.AddStreetFighterClient();

var app = builder.Build();