using Kudiyarov.StreetFighter6.Logic.Interfaces;
using Spectre.Console;

namespace Kudiyarov.StreetFighter6.Logic.Implementations;

public class StyleProvider : IStyleProvider
{
    private const double Step = (double) 1 / 3;

    private readonly Style _badStyle = new(new Color(242, 119, 122));
    private readonly Style _normalStyle = new();
    private readonly Style _goodStyle = new(new Color(153, 204, 153));
    
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