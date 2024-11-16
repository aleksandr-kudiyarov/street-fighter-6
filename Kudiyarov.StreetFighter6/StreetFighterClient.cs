using Kudiyarov.StreetFighter6.Entities;
using Kudiyarov.StreetFighter6.Entities.Response;

namespace Kudiyarov.StreetFighter6;

public class StreetFighterClient(HttpClient httpClient)
{
    private readonly RootRequest _request = GetRequest();

    public async Task<CharacterWinRates[]?> GetResponse()
    {
        const string uri = "https://www.streetfighter.com/6/buckler/api/profile/play/act/characterwinrate";

        var response = await httpClient.PostAsJsonAsync(uri, _request);
        var rootResponse = await response.Content.ReadFromJsonAsync<RootResponse>();
        var result = GetCharacterWinRates(rootResponse);
        return result;
    }

    private static CharacterWinRates[] GetCharacterWinRates(RootResponse? root)
    {
        ArgumentNullException.ThrowIfNull(root);

        var response = root.Response;
        var winRates= response.CharacterWinRates;
        return winRates;
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