using Kudiyarov.StreetFighter6.Common.Entities;
using Kudiyarov.StreetFighter6.Dal.Contracts;
using Kudiyarov.StreetFighter6.Dal.Http.Entities.Entities.GetLeagueInfo.Response;
using Kudiyarov.StreetFighter6.Dal.Http.Entities.Entities.GetWinRates.Response;
using Microsoft.Extensions.Caching.Hybrid;

namespace Kudiyarov.StreetFighter6.Logic;

public class StreetFighterLogic(
    IStreetFighterClient client,
    HybridCache cache)
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
        var winRate = await client.GetWinRate(request, cancellationToken);
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
        
        var leagueInfoTask = GetLeagueInfoImpl(getLeagueInfoRequest, cancellationToken);
        var initialLeagueInfoTask = GetInitialLeagueInfo(getLeagueInfoRequest, cancellationToken);
        
        var leagueInfo = await leagueInfoTask;
        var initialLeagueInfo = await initialLeagueInfoTask;
        
        var response = GetMergedLeagueInfoResponse(leagueInfo, initialLeagueInfo);

        return response;
    }

    private async Task<GetLeagueInfoResponse> GetInitialLeagueInfo(
        GetLeagueInfoRequest request,
        CancellationToken cancellationToken = default)
    {
        var response = await cache.GetOrCreateAsync(
            $"GetInitialLeagueInfo:{request.ProfileId}:{request.SeasonId}",
            async token => await GetInitialLeagueInfoImpl(request, token),
            cancellationToken: cancellationToken);
        
        return response;
    }
    
    private async Task<GetLeagueInfoResponse> GetInitialLeagueInfoImpl(
        GetLeagueInfoRequest request,
        CancellationToken cancellationToken = default)
    {
        var getLeagueInfoResponse = await GetLeagueInfoImpl(request, cancellationToken);

        while (request.SeasonId > 0 && !AllCharactersActual(getLeagueInfoResponse.CharacterLeagueInfos))
        {
            request = request with { SeasonId = request.SeasonId - 1 };
            var previousGetLeagueInfoResponse = await GetLeagueInfoImpl(request, cancellationToken);

            getLeagueInfoResponse = GetMergedLeagueInfoResponse(getLeagueInfoResponse, previousGetLeagueInfoResponse);
        }

        return getLeagueInfoResponse;
    }

    private async Task<GetLeagueInfoResponse> GetLeagueInfoImpl(
        GetLeagueInfoRequest request,
        CancellationToken cancellationToken = default)
    {
        var leagueInfo = await client.GetLeagueInfo(request, cancellationToken);
        ArgumentNullException.ThrowIfNull(leagueInfo);
        return leagueInfo;
    }

    private static bool AllCharactersActual(IEnumerable<CharacterLeagueInfo> characterLeagueInfos)
    {
        var result = characterLeagueInfos.All(info => info.LeagueInfo.LeaguePoint != EmptyLeaguePoints);
        return result;
    }
    
    private static GetLeagueInfoResponse GetMergedLeagueInfoResponse(
        GetLeagueInfoResponse primary,
        GetLeagueInfoResponse secondary)
    {
        var result = Merge(
            primary.CharacterLeagueInfos,
            secondary.CharacterLeagueInfos);

        var response = new GetLeagueInfoResponse
        {
            CharacterLeagueInfos = [.. result]
        };
        
        return response;
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
        var leaguePoint = leagueInfo.LeagueInfo.LeaguePoint;

        var result = new CharacterInfo
        {
            CharacterId = winRate.CharacterId,
            CharacterName = winRate.CharacterName,
            CharacterSort = winRate.CharacterSort,
            WinCount = winRate.WinCount,
            BattleCount = winRate.BattleCount,
            LeaguePoint = leaguePoint == EmptyLeaguePoints
                ? null
                : leaguePoint
        };
        
        return result;
    }
}