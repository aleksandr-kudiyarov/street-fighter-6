using Spectre.Console;

namespace Kudiyarov.StreetFighter6.StyleProviders;

public abstract class StyleProvider<TRequest>
{
    public abstract Style GetStyle(TRequest request);
}