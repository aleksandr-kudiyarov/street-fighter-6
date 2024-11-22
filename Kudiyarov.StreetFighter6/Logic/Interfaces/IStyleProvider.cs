using Spectre.Console;

namespace Kudiyarov.StreetFighter6.Logic.Interfaces;

public interface IStyleProvider
{
    public Style GetStyle(double percentage);
}