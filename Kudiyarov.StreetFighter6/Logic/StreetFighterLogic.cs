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
    private const int EmptyLeaguePoints = -1;
    
    public async Task<GetCharacterInfoResponse> GetCharacterInfos(
        GetCharacterInfoRequest request,
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
        GetCharacterInfoRequest request,
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
        GetCharacterInfoRequest request,
        CancellationToken cancellationToken = default)
    {
        var getLeagueInfoRequest = new GetLeagueInfoRequest
        {
            ProfileId = request.ProfileId,
            SeasonId = request.Season
        };
        
        var currentResponse = await GetLeagueInfoImpl(getLeagueInfoRequest, cancellationToken);

        if (AllCharactersActual(currentResponse.CharacterLeagueInfos))
        {
            return currentResponse;
        }
        
        var older = await GetAggregatedLeagueInfo(request, cancellationToken);
        
        var result = Merge(currentResponse.CharacterLeagueInfos, older.CharacterLeagueInfos);

        var response = new GetLeagueInfoResponse
        {
            CharacterLeagueInfos = result.ToList()
        };
        
        return response;
    }

    private async Task<GetLeagueInfoResponse> GetLeagueInfoImpl(
        GetLeagueInfoRequest request,
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

    private async Task<GetLeagueInfoResponse> GetAggregatedLeagueInfo(
        GetCharacterInfoRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await cache.GetOrCreateAsync(
            $"GetAggregatedLeagueInfo:{request}",
            async _ => await GetAggregatedLeagueInfoImpl(request, cancellationToken));

        ArgumentNullException.ThrowIfNull(result);
        return result;
    }

    private async Task<GetLeagueInfoResponse> GetAggregatedLeagueInfoImpl(
        GetCharacterInfoRequest request,
        CancellationToken cancellationToken = default)
    {
        var season = request.Season - 1;
        
        var getLeagueInfoRequest = new GetLeagueInfoRequest
        {
            ProfileId = request.ProfileId,
            SeasonId = season
        };
        
        var primaryResponse = await GetLeagueInfoImpl(getLeagueInfoRequest, cancellationToken);
        
        while (season >= 0 || AllCharactersActual(primaryResponse.CharacterLeagueInfos))
        {
            getLeagueInfoRequest = new GetLeagueInfoRequest
            {
                ProfileId = request.ProfileId,
                SeasonId = season
            };
            
            var secondaryResponse = await GetLeagueInfoImpl(getLeagueInfoRequest, cancellationToken);
            
            var result = Merge(
                primaryResponse.CharacterLeagueInfos,
                secondaryResponse.CharacterLeagueInfos);

            primaryResponse = new GetLeagueInfoResponse
            {
                CharacterLeagueInfos = result.ToArray()
            };

            season--;
        }

        return primaryResponse;
    }

    private static bool AllCharactersActual(IEnumerable<CharacterLeagueInfo> characterLeagueInfos)
    {
        var result = characterLeagueInfos.All(info => info.LeagueInfo.LeaguePoint != EmptyLeaguePoints);
        return result;
    }

    private static IEnumerable<CharacterLeagueInfo> Merge(
        IEnumerable<CharacterLeagueInfo> primary,
        IEnumerable<CharacterLeagueInfo> secondary)
    {
        var result = primary.Join(secondary,
            primaryInfo => primaryInfo.CharacterId,
            secondaryInfo => secondaryInfo.CharacterId,
            Merge);

        return result;
    }

    private static CharacterLeagueInfo Merge(CharacterLeagueInfo primary, CharacterLeagueInfo secondary)
    {
        var result= primary.LeagueInfo.LeaguePoint != EmptyLeaguePoints
            ? primary
            : secondary;

        return result;
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