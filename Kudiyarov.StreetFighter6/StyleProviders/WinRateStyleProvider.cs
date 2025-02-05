using CommunityToolkit.Diagnostics;
using Kudiyarov.StreetFighter6.Common.Entities;
using Spectre.Console;

namespace Kudiyarov.StreetFighter6.StyleProviders;

public sealed class WinRateStyleProvider : StyleProvider<Percentage>
{
    private readonly Style _badStyle = new(new Color(242, 119, 122));
    private readonly Style _goodStyle = new(new Color(153, 204, 153));
    private readonly Style _normalStyle = new();

    public override Style GetStyle(Percentage percentage)
    {
        var style = percentage.Value switch
        {
            < 0 or > 1 => ThrowHelper.ThrowArgumentOutOfRangeException<Style>(nameof(percentage), percentage, "Value must be between 0 and 1."),
            >= (double) 2 / 3 => _goodStyle,
            >= (double) 1 / 3 or double.NaN => _normalStyle,
            >= (double) 0 / 3 => _badStyle
        };

        return style;
    }
}