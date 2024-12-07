using Kudiyarov.StreetFighter6.Common.Entities;
using Spectre.Console;

namespace Kudiyarov.StreetFighter6.Logic;

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
            LeagueEnum.Rookie => _rookieStyle,
            LeagueEnum.Iron => _ironStyle,
            LeagueEnum.Bronze => _bronzeStyle,
            LeagueEnum.Silver => _silverStyle,
            LeagueEnum.Gold => _goldStyle,
            LeagueEnum.Platinum => _platinumStyle,
            LeagueEnum.Diamond => _diamondStyle,
            LeagueEnum.Master => _masterStyle,
            _ => throw new ArgumentOutOfRangeException(nameof(league), league, null)
        };

        return style;
    }
}