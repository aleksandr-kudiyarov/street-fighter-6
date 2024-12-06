using System.Net.Http.Json;
using Kudiyarov.StreetFighter6.HttpDal.Entities.GetLeagueInfo;
using Kudiyarov.StreetFighter6.HttpDal.Entities.GetLeagueInfo.Response;
using Kudiyarov.StreetFighter6.HttpDal.Entities.GetWinRates;
using Kudiyarov.StreetFighter6.HttpDal.Entities.GetWinRates.Response;

namespace Kudiyarov.StreetFighter6.HttpDal;

public class StreetFighterClient(HttpClient httpClient)
{
    private const string Locale = "en";
    private const long TargetShortId = 2991759546;
    private const int TargetSeasonId = -1;
    
    private readonly GetWinRateRequest _getWinRateRequest = GetWinRateRequest();
    private readonly GetLeagueInfoRequest _getLeagueInfoRequest = GetLeagueInfoRequest();

    public async Task<GetWinRateResponse> GetWinRate(CancellationToken cancellationToken = default)
    {
        const string uri = "https://www.streetfighter.com/6/buckler/api/profile/play/act/characterwinrate";

        var response = await httpClient.PostAsJsonAsync(uri, _getWinRateRequest, cancellationToken: cancellationToken);
        response.EnsureSuccessStatusCode();
        var root = await response.Content.ReadFromJsonAsync<GetWinRateResponseRoot>(cancellationToken: cancellationToken);
        ArgumentNullException.ThrowIfNull(root);
        return root.Response;
    }

    public async Task<GetLeagueInfoResponse> GetLeagueInfo(CancellationToken cancellationToken = default)
    {
        const string uri = "https://www.streetfighter.com/6/buckler/api/profile/play/act/leagueinfo";

        var response = await httpClient.PostAsJsonAsync(uri, _getLeagueInfoRequest, cancellationToken: cancellationToken);
        response.EnsureSuccessStatusCode();
        var root = await response.Content.ReadFromJsonAsync<GetLeagueInfoResponseRoot>(cancellationToken: cancellationToken);
        ArgumentNullException.ThrowIfNull(root);
        return root.Response;
    }

    private static GetWinRateRequest GetWinRateRequest()
    {
        var rootRequest = new GetWinRateRequest
        {
            TargetShortId = TargetShortId,
            TargetSeasonId = TargetSeasonId,
            TargetModeId = 2,
            Locale = Locale
        };
        
        return rootRequest;
    }

    private static GetLeagueInfoRequest GetLeagueInfoRequest()
    {
        var request = new GetLeagueInfoRequest
        {
            TargetShortId = TargetShortId,
            TargetSeasonId = TargetSeasonId,
            Locale = Locale,
            Peak = true
        };
        
        return request;
    }
}