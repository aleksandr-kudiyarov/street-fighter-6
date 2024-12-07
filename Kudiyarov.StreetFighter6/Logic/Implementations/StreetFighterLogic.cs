using Kudiyarov.StreetFighter6.Common;
using Kudiyarov.StreetFighter6.HttpDal;
using Kudiyarov.StreetFighter6.HttpDal.Entities.GetLeagueInfo.Response;
using Kudiyarov.StreetFighter6.HttpDal.Entities.GetWinRates.Response;
using Microsoft.Extensions.Caching.Memory;

namespace Kudiyarov.StreetFighter6.Logic.Implementations;

public class StreetFighterLogic(
    StreetFighterClient client,
    IMemoryCache cache)
{
    public async Task<IEnumerable<CharacterInfo>> GetCharacterInfos(CancellationToken cancellationToken = default)
    {
        var winRate = await GetWinRate(cancellationToken);
        var leagueInfo = await GetLeagueInfo(cancellationToken);

        var response = winRate.CharacterWinRate.Join(leagueInfo.CharacterLeagueInfos,
            left => left.CharacterId,
            right => right.CharacterId,
            GetCharacterInfo);
        
        return response;
    }

    private async Task<GetWinRateResponse> GetWinRate(CancellationToken cancellationToken = default)
    {
        var winRate = await cache.GetOrCreateAsync(
            "GetWinRate",
            async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(5);
                return await client.GetWinRate(cancellationToken);
            });
        
        ArgumentNullException.ThrowIfNull(winRate);
        return winRate;
    }

    private async Task<GetLeagueInfoResponse> GetLeagueInfo(CancellationToken cancellationToken = default)
    {
        var leagueInfo = await cache.GetOrCreateAsync(
            "GetLeagueInfo",
            async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(5);
                return await client.GetLeagueInfo(cancellationToken);
            });
        
        ArgumentNullException.ThrowIfNull(leagueInfo);
        return leagueInfo;
    }

    private static CharacterInfo GetCharacterInfo(CharacterWinRates winRate, CharacterLeagueInfo leagueInfo)
    {
        var result = new CharacterInfo
        {
            CharacterId = winRate.CharacterId,
            CharacterName = winRate.CharacterName,
            CharacterSort = winRate.CharacterSort,
            WinCount = winRate.WinCount,
            BattleCount = winRate.BattleCount,
            LeaguePoint = leagueInfo.LeagueInfo.LeaguePoint
        };
        
        return result;
    }
}