using System.Net.Http.Json;
using Kudiyarov.StreetFighter6.Common.Entities;
using Kudiyarov.StreetFighter6.HttpDal.Entities.GetLeagueInfo.Request;
using Kudiyarov.StreetFighter6.HttpDal.Entities.GetWinRates.Request;
using Kudiyarov.StreetFighter6.HttpDal.Entities.GetWinRates.Response;

namespace Kudiyarov.StreetFighter6.HttpDal;

public class StreetFighterClient(HttpClient httpClient)
{
    private const string Locale = "en";
    private const long TargetShortId = 2991759546;
    private const int TargetSeasonId = -1;
    
    private readonly RootRequest _request = GetWinRateRequest();

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
        var winRates = source.CharacterWinRate
            .Where(IsCharacter)
            .OrderBy(character => character.CharacterSort)
            .Select(Map);
        
        var destination = new GetWinRatesResponse
        {
            CharacterInfos = winRates
        };
        
        return destination;
    }

    private static bool IsCharacter(CharacterWinRates source)
    {
        const int any = 253;
        const int random = 254;
        var isCharacter = source.CharacterId is not (any or random);
        return isCharacter;
    }

    private static CharacterInfo Map(CharacterWinRates source)
    {
        var destination = new CharacterInfo
        {
            Name = source.CharacterName,
            Wins = source.WinCount,
            Battles = source.BattleCount
        };
        
        return destination;
    }

    private static RootRequest GetWinRateRequest()
    {
        var rootRequest = new RootRequest
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