using System.Net.Http.Json;
using Kudiyarov.StreetFighter6.Common.Entities;
using Kudiyarov.StreetFighter6.HttpDal.Entities.Request;
using Kudiyarov.StreetFighter6.HttpDal.Entities.Response;

namespace Kudiyarov.StreetFighter6.HttpDal;

public class StreetFighterClient(HttpClient httpClient)
{
    private readonly RootRequest _request = GetRequest();

    public async Task<GetWinRatesResponse> GetResponse(CancellationToken cancellationToken = default)
    {
        const string uri = "https://www.streetfighter.com/6/buckler/api/profile/play/act/characterwinrate";

        var response = await httpClient.PostAsJsonAsync(uri, _request, cancellationToken: cancellationToken);
        var rootResponse = await response.Content.ReadFromJsonAsync<RootResponse>(cancellationToken: cancellationToken);
        var result = GetWinRates(rootResponse);
        return result;
    }

    private static GetWinRatesResponse GetWinRates(RootResponse? root)
    {
        ArgumentNullException.ThrowIfNull(root);

        var response = Map(root.Response);
        return response;
    }

    private static GetWinRatesResponse Map(Response source)
    {
        var winRates = source.CharacterWinRates.Select(Map);
        
        var destination = new GetWinRatesResponse
        {
            WinRates = winRates
        };
        
        return destination;
    }

    private static WinRate Map(CharacterWinRates source)
    {
        var destination = new WinRate
        {
            CharacterName = source.CharacterName,
            WinsCount = source.WinCount,
            BattlesCount = source.BattleCount
        };
        
        return destination;
    }

    private static RootRequest GetRequest()
    {
        var rootRequest = new RootRequest
        {
            TargetShortId = 2991759546,
            TargetSeasonId = -1,
            TargetModeId = 2,
            Locale = "en"
        };
        
        return rootRequest;
    }
}