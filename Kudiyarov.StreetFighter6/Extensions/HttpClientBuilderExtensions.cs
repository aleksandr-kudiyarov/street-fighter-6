using Kudiyarov.StreetFighter6.HttpDal;
using Microsoft.Net.Http.Headers;

namespace Kudiyarov.StreetFighter6.Extensions;

public static class HttpClientBuilderExtensions
{
    public static void AddStreetFighterClient(
        this IServiceCollection services,
        Authentication options)
    {
        services
            .AddHttpClient<StreetFighterClient>(client =>
            {
                client.DefaultRequestHeaders.Add(HeaderNames.UserAgent, options.UserAgent);
                client.DefaultRequestHeaders.Add(HeaderNames.Cookie, options.Cookie);
            })
            .AddStandardResilienceHandler();
    }
}
