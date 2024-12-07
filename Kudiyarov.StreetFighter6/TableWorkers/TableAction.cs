using Kudiyarov.StreetFighter6.Common;
using Kudiyarov.StreetFighter6.Logic.Implementations;
using Spectre.Console;

namespace Kudiyarov.StreetFighter6.TableWorkers;

public abstract class TableAction(StreetFighterLogic client)
{
    public async Task Invoke(Table table)
    {
        var response = await client.GetCharacterInfos();
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

    protected string GetLeagueLevel(int leagueLevel)
    {
        var level = leagueLevel switch
        {
            5 => "[\u2605\u2605\u2605\u2605\u2605]",
            4 => "[\u2605\u2605\u2605\u2605 ]",
            3 => "[\u2605\u2605\u2605  ]",
            2 => "[\u2605\u2605   ]",
            1 => "[\u2605    ]",
            _ => throw new ArgumentOutOfRangeException(nameof(leagueLevel), leagueLevel, null)
        };

        return level;
    }

    protected LeagueInfo GetLeagueInfo(int leaguePoints)
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
            _ => throw new ArgumentOutOfRangeException(nameof(leaguePoints), leaguePoints, null)
        };

        return leagueInfo;
    }
}

public record struct LeagueInfo(LeagueEnum League, int Level);

public enum LeagueEnum
{
    Rookie,
    Iron,
    Bronze,
    Silver,
    Gold,
    Platinum,
    Diamond,
    Master
}