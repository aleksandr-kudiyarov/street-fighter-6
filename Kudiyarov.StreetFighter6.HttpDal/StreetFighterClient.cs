using System.Net.Http.Json;
using Kudiyarov.StreetFighter6.Common.Entities;
using Kudiyarov.StreetFighter6.HttpDal.Entities.GetLeagueInfo;
using Kudiyarov.StreetFighter6.HttpDal.Entities.GetLeagueInfo.Response;
using Kudiyarov.StreetFighter6.HttpDal.Entities.GetWinRates;
using Kudiyarov.StreetFighter6.HttpDal.Entities.GetWinRates.Response;

namespace Kudiyarov.StreetFighter6.HttpDal;

public class StreetFighterClient(HttpClient httpClient)
{
    private const string Locale = "en";
    private const int TargetSeasonId = -1;
    
    public async Task<GetWinRateResponse?> GetWinRate(
        GetCharacterInfosRequest request,
        CancellationToken cancellationToken = default)
    {
        const string uri = "https://www.streetfighter.com/6/buckler/api/profile/play/act/characterwinrate";
        
        var apiRequest = GetWinRateRequest(request);
        var response = await httpClient.PostAsJsonAsync(uri, apiRequest, cancellationToken);
        response.EnsureSuccessStatusCode();
        var root = await response.Content.ReadFromJsonAsync<GetWinRateResponseRoot>(cancellationToken: cancellationToken);
        return root?.Response;
    }

    public async Task<GetLeagueInfoResponse?> GetLeagueInfo(
        GetCharacterInfosRequest request,
        CancellationToken cancellationToken = default)
    {
        const string uri = "https://www.streetfighter.com/6/buckler/api/profile/play/act/leagueinfo";

        var apiRequest = GetLeagueInfoRequest(request);
        var response = await httpClient.PostAsJsonAsync(uri, apiRequest, cancellationToken);
        response.EnsureSuccessStatusCode();
        var root = await response.Content.ReadFromJsonAsync<GetLeagueInfoResponseRoot>(cancellationToken: cancellationToken);
        return root?.Response;
    }

    private static GetWinRateRequest GetWinRateRequest(GetCharacterInfosRequest request)
    {
        var apiRequest = new GetWinRateRequest
        {
            TargetShortId = request.ProfileId,
            TargetSeasonId = TargetSeasonId,
            TargetModeId = 2,
            Locale = Locale
        };
        
        return apiRequest;
    }

    private static GetLeagueInfoRequest GetLeagueInfoRequest(GetCharacterInfosRequest request)
    {
        var apiRequest = new GetLeagueInfoRequest
        {
            TargetShortId = request.ProfileId,
            TargetSeasonId = TargetSeasonId,
            Locale = Locale,
            Peak = true
        };
        
        return apiRequest;
    }
}