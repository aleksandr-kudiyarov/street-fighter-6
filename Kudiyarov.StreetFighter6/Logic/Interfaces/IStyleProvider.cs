using Kudiyarov.StreetFighter6.Common.Entities;
using Spectre.Console;

namespace Kudiyarov.StreetFighter6.Logic.Interfaces;

public interface IStyleProvider
{
    Style GetWinRateStyle(double percentage);
    Style GetLeagueStyle(LeagueEnum league);
}