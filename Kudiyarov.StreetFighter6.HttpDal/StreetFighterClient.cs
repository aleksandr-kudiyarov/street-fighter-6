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
        const int totalSeasonsId = -1;
        
        var apiRequest = new GetWinRateApiRequest
        {
            TargetShortId = request.ProfileId,
            TargetSeasonId = totalSeasonsId,
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
            Peak = false,
            Locale = Locale
        };
        
        return apiRequest;
    }
}