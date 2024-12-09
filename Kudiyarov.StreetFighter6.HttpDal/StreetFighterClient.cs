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
        GetCharacterInfoRequest request,
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
        GetLeagueInfoRequest request,
        CancellationToken cancellationToken = default)
    {
        const string uri = "https://www.streetfighter.com/6/buckler/api/profile/play/act/leagueinfo";

        var apiRequest = GetLeagueInfoRequest(request);
        var response = await httpClient.PostAsJsonAsync(uri, apiRequest, cancellationToken);
        response.EnsureSuccessStatusCode();
        var root = await response.Content.ReadFromJsonAsync<GetLeagueInfoResponseRoot>(cancellationToken: cancellationToken);
        return root?.Response;
    }

    private static GetWinRateApiRequest GetWinRateRequest(GetCharacterInfoRequest request)
    {
        var apiRequest = new GetWinRateApiRequest
        {
            TargetShortId = request.ProfileId,
            TargetSeasonId = TargetSeasonId,
            TargetModeId = 2,
            Locale = Locale
        };
        
        return apiRequest;
    }

    private static GetLeagueInfoApiRequest GetLeagueInfoRequest(GetLeagueInfoRequest request)
    {
        var apiRequest = new GetLeagueInfoApiRequest
        {
            TargetShortId = request.ProfileId,
            TargetSeasonId = request.SeasonId,
            Locale = Locale,
            Peak = false
        };
        
        return apiRequest;
    }
}