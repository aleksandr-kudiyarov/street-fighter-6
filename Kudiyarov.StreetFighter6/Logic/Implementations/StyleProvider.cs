using Kudiyarov.StreetFighter6.Common.Entities;
using Kudiyarov.StreetFighter6.Logic.Interfaces;
using Spectre.Console;

namespace Kudiyarov.StreetFighter6.Logic.Implementations;

public class StyleProvider : IStyleProvider
{
    private const double Step = (double) 1 / 3;

    private readonly Style _badStyle = new(new Color(242, 119, 122));
    private readonly Style _normalStyle = new();
    private readonly Style _goodStyle = new(new Color(153, 204, 153));
    
    private readonly Style _rookieStyle    = new(new Color(128, 128, 128));
    private readonly Style _ironStyle      = new(new Color(169, 169, 169));
    private readonly Style _bronzeStyle    = new(new Color(205, 127, 050));
    private readonly Style _silverStyle    = new(new Color(192, 192, 192));
    private readonly Style _goldStyle      = new(new Color(255, 215, 000));
    private readonly Style _platinumStyle  = new(new Color(255, 205, 000));
    private readonly Style _diamondStyle   = new(new Color(000, 191, 255));
    private readonly Style _masterStyle    = new(new Color(128, 000, 128));
    
    public Style GetWinRateStyle(double percentage)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(percentage, nameof(percentage));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(percentage, 1, nameof(percentage));

        var style = percentage switch
        {
            > 1 => throw new ArgumentOutOfRangeException(nameof(percentage), percentage, "Value must be between 0 and 1."),
            >= Step * 2 => _goodStyle,
            >= Step * 1 or double.NaN => _normalStyle,
            >= Step * 0 => _badStyle,
            < 0 => throw new ArgumentOutOfRangeException(nameof(percentage), percentage, "Value must be between 0 and 1."),
        };
        
        return style;
    }
    
    public Style GetLeagueStyle(LeagueEnum league)
    {
        var style = league switch
        {
            LeagueEnum.Rookie    => _rookieStyle,
            LeagueEnum.Iron      => _ironStyle,
            LeagueEnum.Bronze    => _bronzeStyle,
            LeagueEnum.Silver    => _silverStyle,
            LeagueEnum.Gold      => _goldStyle,
            LeagueEnum.Platinum  => _platinumStyle,
            LeagueEnum.Diamond   => _diamondStyle,
            LeagueEnum.Master    => _masterStyle,
            _ => throw new ArgumentOutOfRangeException(nameof(league), league, null)
        };
        
        return style;
    }
}