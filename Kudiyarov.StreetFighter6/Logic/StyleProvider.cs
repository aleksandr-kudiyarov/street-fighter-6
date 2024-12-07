using Spectre.Console;

namespace Kudiyarov.StreetFighter6.Logic;

public abstract class StyleProvider<TRequest>
{
    public abstract Style GetStyle(TRequest request);
}