using Kudiyarov.StreetFighter6.HttpDal;
using Microsoft.Net.Http.Headers;

namespace Kudiyarov.StreetFighter6.Extensions;

public static class HttpClientBuilderExtensions
{
    public static void AddStreetFighterClient(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var options = GetAuthenticationOptions(configuration);

        services
            .AddHttpClient<StreetFighterClient>(client =>
            {
                client.DefaultRequestHeaders.Add(HeaderNames.UserAgent, options.UserAgent);
                client.DefaultRequestHeaders.Add(HeaderNames.Cookie, options.Cookie);
            })
            .AddStandardResilienceHandler();
    }

    private static AuthenticationOptions GetAuthenticationOptions(IConfiguration configuration)
    {
        var options = configuration
            .GetSection("Authentication")
            .Get<AuthenticationOptions>();

        ArgumentNullException.ThrowIfNull(options);

        return options;
    }
    
    private record AuthenticationOptions(string UserAgent, string Cookie);
}
