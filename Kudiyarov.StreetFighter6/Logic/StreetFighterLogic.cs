using Kudiyarov.StreetFighter6.Common.Entities;
using Kudiyarov.StreetFighter6.Dal.Contracts;
using Kudiyarov.StreetFighter6.Dal.Http.Entities.Entities.GetLeagueInfo.Response;
using Kudiyarov.StreetFighter6.Dal.Http.Entities.Entities.GetWinRates.Response;

namespace Kudiyarov.StreetFighter6.Logic;

public class StreetFighterLogic(
    IStreetFighterClient client)
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
        
        var getLeagueInfoResponse = await GetLeagueInfoImpl(getLeagueInfoRequest, cancellationToken);

        while (getLeagueInfoRequest.SeasonId > 0 && !AllCharactersActual(getLeagueInfoResponse.CharacterLeagueInfos))
        {
            getLeagueInfoRequest = getLeagueInfoRequest with { SeasonId = getLeagueInfoRequest.SeasonId - 1 };
            var previousGetLeagueInfoResponse = await GetLeagueInfoImpl(getLeagueInfoRequest, cancellationToken);

            var result = Merge(
                getLeagueInfoResponse.CharacterLeagueInfos,
                previousGetLeagueInfoResponse.CharacterLeagueInfos);

            getLeagueInfoResponse = new GetLeagueInfoResponse
            {
                CharacterLeagueInfos = [.. result]
            };
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