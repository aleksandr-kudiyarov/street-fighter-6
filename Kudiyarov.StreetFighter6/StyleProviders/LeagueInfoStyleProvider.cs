using CommunityToolkit.Diagnostics;
using Kudiyarov.StreetFighter6.Common.Entities;
using Spectre.Console;

namespace Kudiyarov.StreetFighter6.StyleProviders;

public sealed class LeagueInfoStyleProvider : StyleProvider<LeagueEnum>
{
    private readonly Style _bronzeStyle = new(new Color(205, 127, 050));
    private readonly Style _diamondStyle = new(new Color(000, 191, 255));
    private readonly Style _goldStyle = new(new Color(255, 215, 000));
    private readonly Style _ironStyle = new(new Color(169, 169, 169));
    private readonly Style _masterStyle = new(new Color(128, 000, 128));
    private readonly Style _platinumStyle = new(new Color(255, 205, 000));
    private readonly Style _rookieStyle = new(new Color(128, 128, 128));
    private readonly Style _silverStyle = new(new Color(192, 192, 192));

    public override Style GetStyle(LeagueEnum league)
    {
        var style = league switch
        {
            LeagueEnum.Master => _masterStyle,
            LeagueEnum.Diamond => _diamondStyle,
            LeagueEnum.Platinum => _platinumStyle,
            LeagueEnum.Gold => _goldStyle,
            LeagueEnum.Silver => _silverStyle,
            LeagueEnum.Bronze => _bronzeStyle,
            LeagueEnum.Iron => _ironStyle,
            LeagueEnum.Rookie => _rookieStyle,
            _ => ThrowHelper.ThrowArgumentOutOfRangeException<Style>(nameof(league), league, "Value must be defined")
        };

        return style;
    }
}