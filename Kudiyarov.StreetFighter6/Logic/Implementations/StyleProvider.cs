using Kudiyarov.StreetFighter6.Logic.Interfaces;
using Spectre.Console;

namespace Kudiyarov.StreetFighter6.Logic.Implementations;

public class StyleProvider : IStyleProvider
{
    private const double Step = (double) 1 / 3;

    private readonly Style _badStyle = new(Color.Red);
    private readonly Style _normalStyle = new(Color.DarkSlateGray1);
    private readonly Style _goodStyle = new(Color.Green);
    
    public Style GetStyle(double percentage)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(percentage, nameof(percentage));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(percentage, 1, nameof(percentage));

        var style = percentage switch
        {
            >= Step * 2 => _goodStyle,
            >= Step * 1 or double.NaN => _normalStyle,
            >= Step * 0 => _badStyle
        };
        
        return style;
    }
}