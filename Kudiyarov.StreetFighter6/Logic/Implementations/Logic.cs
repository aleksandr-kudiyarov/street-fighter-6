using Kudiyarov.StreetFighter6.HttpDal;
using Kudiyarov.StreetFighter6.HttpDal.Entities.GetLeagueInfo.Response;
using Kudiyarov.StreetFighter6.HttpDal.Entities.GetWinRates.Response;
using Microsoft.Extensions.Caching.Memory;

namespace Kudiyarov.StreetFighter6.Logic.Implementations;

public interface ILogic
{
    
}

public class Logic(
    StreetFighterClient client,
    IMemoryCache cache)
{
    private async Task<GetWinRateResponse> GetWinRate(CancellationToken cancellationToken = default)
    {
        var winRate = await cache.GetOrCreateAsync(
            "GetWinRate",
            async entry => await client.GetWinRate(cancellationToken));
        
        ArgumentNullException.ThrowIfNull(winRate);
        return winRate;
    }

    private async Task<GetLeagueInfoResponse> GetLeagueInfo(CancellationToken cancellationToken = default)
    {
        var leagueInfo = await cache.GetOrCreateAsync(
            "GetLeagueInfo",
            async entry => await client.GetLeagueInfo(cancellationToken));
        
        ArgumentNullException.ThrowIfNull(leagueInfo);
        return leagueInfo;
    }
}