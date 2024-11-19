using System.Net.Http.Json;
using Kudiyarov.StreetFighter6.HttpDal.Entities.Request;
using Kudiyarov.StreetFighter6.HttpDal.Entities.Response;

namespace Kudiyarov.StreetFighter6.HttpDal;

public class StreetFighterClient(HttpClient httpClient)
{
    private readonly RootRequest _request = GetRequest();

    public async Task<Response> GetResponse(CancellationToken cancellationToken = default)
    {
        const string uri = "https://www.streetfighter.com/6/buckler/api/profile/play/act/characterwinrate";

        var response = await httpClient.PostAsJsonAsync(uri, _request, cancellationToken: cancellationToken);
        var rootResponse = await response.Content.ReadFromJsonAsync<RootResponse>(cancellationToken: cancellationToken);
        var result = GetCharacterWinRates(rootResponse);
        return result;
    }

    private static Response GetCharacterWinRates(RootResponse? root)
    {
        ArgumentNullException.ThrowIfNull(root);

        var response = root.Response;
        return response;
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