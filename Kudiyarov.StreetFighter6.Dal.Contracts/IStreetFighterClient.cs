using Kudiyarov.StreetFighter6.Common.Entities;
using Kudiyarov.StreetFighter6.Dal.Http.Entities.Entities.GetLeagueInfo.Response;
using Kudiyarov.StreetFighter6.Dal.Http.Entities.Entities.GetWinRates.Response;

namespace Kudiyarov.StreetFighter6.Dal.Contracts;

public interface IStreetFighterClient
{
    Task<GetWinRateResponse?> GetWinRate(
        GetCharacterInfoRequest request,
        CancellationToken cancellationToken = default);

    Task<GetLeagueInfoResponse?> GetLeagueInfo(
        GetLeagueInfoRequest request,
        CancellationToken cancellationToken = default);
}