using Kudiyarov.StreetFighter6.Common.Entities;
using Kudiyarov.StreetFighter6.HttpDal;
using Kudiyarov.StreetFighter6.HttpDal.Entities.GetLeagueInfo.Response;
using Kudiyarov.StreetFighter6.HttpDal.Entities.GetWinRates.Response;
using Microsoft.Extensions.Caching.Memory;

namespace Kudiyarov.StreetFighter6.Logic;

public class StreetFighterLogic(
    StreetFighterClient client,
    IMemoryCache cache)
{
    public async Task<GetCharacterInfoResponse> GetCharacterInfos(
        GetCharacterInfosRequest request,
        CancellationToken cancellationToken = default)
    {
        var winRateTask = GetWinRate(request, cancellationToken);
        var leagueInfoTask = GetLeagueInfo(request, cancellationToken);

        var winRate = await winRateTask;
        var leagueInfo = await leagueInfoTask;
        
        var characterInfos = winRate.CharacterWinRate.Join(leagueInfo.CharacterLeagueInfos,
            left => left.CharacterId,
            right => right.CharacterId,
            GetCharacterInfo);

        var response = new GetCharacterInfoResponse
        {
            CharacterInfos = characterInfos
        };
        
        return response;
    }

    private async Task<GetWinRateResponse> GetWinRate(
        GetCharacterInfosRequest request,
        CancellationToken cancellationToken = default)
    {
        var winRate = await cache.GetOrCreateAsync(
            $"GetWinRate:{request}",
            async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(5);
                return await client.GetWinRate(request, cancellationToken);
            });
        
        ArgumentNullException.ThrowIfNull(winRate);
        return winRate;
    }

    private async Task<GetLeagueInfoResponse> GetLeagueInfo(
        GetCharacterInfosRequest request,
        CancellationToken cancellationToken = default)
    {
        var leagueInfo = await cache.GetOrCreateAsync(
            $"GetLeagueInfo:{request}",
            async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(5);
                return await client.GetLeagueInfo(request, cancellationToken);
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