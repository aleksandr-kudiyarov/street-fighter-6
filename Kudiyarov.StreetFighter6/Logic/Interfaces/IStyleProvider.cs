using Kudiyarov.StreetFighter6.TableWorkers;
using Spectre.Console;

namespace Kudiyarov.StreetFighter6.Logic.Interfaces;

public interface IStyleProvider
{
    Style GetWinRateStyle(double percentage);
    Style GetLeagueStyle(LeagueEnum league);
}