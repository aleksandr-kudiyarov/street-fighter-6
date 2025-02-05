using CommunityToolkit.Diagnostics;
using Kudiyarov.StreetFighter6.Common.Entities;
using Kudiyarov.StreetFighter6.Logic;
using Kudiyarov.StreetFighter6.StyleProviders;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace Kudiyarov.StreetFighter6.TableWorkers;

public abstract class TableAction(
    StreetFighterLogic client,
    StyleProvider<LeagueEnum> leagueStyleProvider,
    StyleProvider<Percentage> percentageStyleProvider)
{
    private static readonly IRenderable EmptyText = new Text(string.Empty);
    
    public async Task Invoke(GetCharacterInfoRequest request, Table table)
    {
        var response = await client.GetCharacterInfos(request);
        Action(table, response);
    }

    protected abstract void Action(Table table, GetCharacterInfoResponse response);

    protected static IEnumerable<CharacterInfo> GetCharacterWinRates(IEnumerable<CharacterInfo> winRates)
    {
        return winRates.Where(IsCharacter);
    }
    
    private static bool IsCharacter(CharacterInfo source)
    {
        const int any = 253;
        const int random = 254;
        var isCharacter = source.CharacterId is not (any or random);
        return isCharacter;
    }
    
    protected static IRenderable GetWins(CharacterInfo character)
    {
        var wins = character.WinCount;
        return new Text(wins.ToString());
    }
    
    protected static IRenderable GetBattles(CharacterInfo character)
    {
        var battles = character.BattleCount;
        return new Text(battles.ToString());
    }

    protected IRenderable GetWinsPercentage(CharacterInfo character)
    {
        if (character.BattleCount == 0)
        {
            return EmptyText;
        }
        
        var winsPercentage = (double) character.WinCount / character.BattleCount;
        var winsPercentageStyle = percentageStyleProvider.GetStyle(winsPercentage);
        return new Text(winsPercentage.ToString("P1"), winsPercentageStyle);
    }

    protected IRenderable GetLeaguePoints(CharacterInfo character)
    {
        var leaguePoints = character.LeaguePoint;

        if (leaguePoints == null)
        {
            return EmptyText;
        }
        
        return new Text(leaguePoints.Value.ToString());
    }
    
    protected IRenderable GetLeagueLevel(CharacterInfo character)
    {
        var leaguePoints = character.LeaguePoint;

        if (leaguePoints == null)
        {
            return EmptyText;
        }
        
        var leagueInfo = GetLeagueInfo(leaguePoints.Value);
        var leagueLevel = GetLeagueLevel(leagueInfo.Level);
        var leagueStyle = leagueStyleProvider.GetStyle(leagueInfo.League);
        return new Text(leagueLevel, leagueStyle);
    }

    private static string GetLeagueLevel(int leagueLevel)
    {
        var level = leagueLevel switch
        {
            5 => """[★★★★★]""",
            4 => """[★★★★ ]""",
            3 => """[★★★  ]""",
            2 => """[★★   ]""",
            1 => """[★    ]""",
            _ => ThrowHelper.ThrowArgumentOutOfRangeException<string>(nameof(leagueLevel), leagueLevel, "Value must be between 1 and 5.")
        };

        return level;
    }

    private static LeagueInfo GetLeagueInfo(int leaguePoints)
    {
        
        var leagueInfo = leaguePoints switch
        {
            >= 25000 => new LeagueInfo(LeagueEnum.Master, 1),
            
            >= 23800 => new LeagueInfo(LeagueEnum.Diamond, 5),
            >= 22600 => new LeagueInfo(LeagueEnum.Diamond, 4),
            >= 21400 => new LeagueInfo(LeagueEnum.Diamond, 3),
            >= 20200 => new LeagueInfo(LeagueEnum.Diamond, 2),
            >= 19000 => new LeagueInfo(LeagueEnum.Diamond, 1),
            
            >= 17800 => new LeagueInfo(LeagueEnum.Platinum, 5),
            >= 16600 => new LeagueInfo(LeagueEnum.Platinum, 4),
            >= 15400 => new LeagueInfo(LeagueEnum.Platinum, 3),
            >= 14200 => new LeagueInfo(LeagueEnum.Platinum, 2),
            >= 13000 => new LeagueInfo(LeagueEnum.Platinum, 1),
            
            >= 12200 => new LeagueInfo(LeagueEnum.Gold, 5),
            >= 11400 => new LeagueInfo(LeagueEnum.Gold, 4),
            >= 10600 => new LeagueInfo(LeagueEnum.Gold, 3),
            >= 9800 => new LeagueInfo(LeagueEnum.Gold, 2),
            >= 9000 => new LeagueInfo(LeagueEnum.Gold, 1),
            
            >= 8200 => new LeagueInfo(LeagueEnum.Silver, 5),
            >= 7400 => new LeagueInfo(LeagueEnum.Silver, 4),
            >= 6600 => new LeagueInfo(LeagueEnum.Silver, 3),
            >= 5800 => new LeagueInfo(LeagueEnum.Silver, 2),
            >= 5000 => new LeagueInfo(LeagueEnum.Silver, 1),
            
            >= 4600 => new LeagueInfo(LeagueEnum.Bronze, 5),
            >= 4200 => new LeagueInfo(LeagueEnum.Bronze, 4),
            >= 3800 => new LeagueInfo(LeagueEnum.Bronze, 3),
            >= 3400 => new LeagueInfo(LeagueEnum.Bronze, 2),
            >= 3000 => new LeagueInfo(LeagueEnum.Bronze, 1),
            
            >= 2600 => new LeagueInfo(LeagueEnum.Iron, 5),
            >= 2200 => new LeagueInfo(LeagueEnum.Iron, 4),
            >= 1800 => new LeagueInfo(LeagueEnum.Iron, 3),
            >= 1400 => new LeagueInfo(LeagueEnum.Iron, 2),
            >= 1000 => new LeagueInfo(LeagueEnum.Iron, 1),
            
            >= 800 => new LeagueInfo(LeagueEnum.Rookie, 5),
            >= 600 => new LeagueInfo(LeagueEnum.Rookie, 4),
            >= 400 => new LeagueInfo(LeagueEnum.Rookie, 3),
            >= 200 => new LeagueInfo(LeagueEnum.Rookie, 2),
            >= 0 => new LeagueInfo(LeagueEnum.Rookie, 1),
            
            < 0 => ThrowHelper.ThrowArgumentOutOfRangeException<LeagueInfo>(nameof(leaguePoints), leaguePoints, "Value must be greater than 0"),
        };

        return leagueInfo;
    }
}