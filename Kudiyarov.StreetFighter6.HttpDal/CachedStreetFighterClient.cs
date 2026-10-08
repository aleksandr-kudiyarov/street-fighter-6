using Kudiyarov.StreetFighter6.Common.Entities;
using Kudiyarov.StreetFighter6.Dal.Contracts;
using Kudiyarov.StreetFighter6.Dal.Http.Entities.Entities.GetLeagueInfo.Response;
using Kudiyarov.StreetFighter6.Dal.Http.Entities.Entities.GetWinRates.Response;
using Microsoft.Extensions.Caching.Hybrid;

namespace Kudiyarov.StreetFighter6.HttpDal;

public class CachedStreetFighterClient(
    IStreetFighterClient client,
    HybridCache cache) : IStreetFighterClient
{
    private readonly HybridCacheEntryOptions _cacheOptions = new()
    {
        Expiration = TimeSpan.FromSeconds(5),
        LocalCacheExpiration = TimeSpan.FromSeconds(5)
    };
    
    public async Task<GetWinRateResponse?> GetWinRate(
        GetCharacterInfoRequest request,
        CancellationToken cancellationToken = default)
    {
        var winRate = await cache.GetOrCreateAsync(
            $"GetWinRate:{request.ProfileId}:{request.Season}",
            async token => await client.GetWinRate(request, token),
            _cacheOptions,
            cancellationToken: cancellationToken);
        
        ArgumentNullException.ThrowIfNull(winRate);
        return winRate;
    }

    public async Task<GetLeagueInfoResponse?> GetLeagueInfo(
        GetLeagueInfoRequest request,
        CancellationToken cancellationToken = default)
    {
        var leagueInfo = await cache.GetOrCreateAsync(
            $"GetLeagueInfo:{request.ProfileId}:{request.SeasonId}",
            async token => await client.GetLeagueInfo(request, token),
            _cacheOptions,
            cancellationToken: cancellationToken);
        
        ArgumentNullException.ThrowIfNull(leagueInfo);
        return leagueInfo;
    }
}
