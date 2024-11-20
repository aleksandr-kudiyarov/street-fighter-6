using Kudiyarov.StreetFighter6.HttpDal;
using Microsoft.Net.Http.Headers;

namespace Kudiyarov.StreetFighter6.Extensions;

public static class HttpClientBuilderExtensions
{
    public static void AddStreetFighterClient(this HostApplicationBuilder builder)
    {
        var options = GetAuthenticationOptions(builder);

        builder.Services
            .AddHttpClient<StreetFighterClient>(client =>
            {
                client.DefaultRequestHeaders.Add(HeaderNames.UserAgent, options.UserAgent);
                client.DefaultRequestHeaders.Add(HeaderNames.Cookie, options.Cookie);
            })
            .AddStandardResilienceHandler();
    }

    private static AuthenticationOptions GetAuthenticationOptions(HostApplicationBuilder builder)
    {
        var options = builder.Configuration
            .GetSection("Authentication")
            .Get<AuthenticationOptions>();

        ArgumentNullException.ThrowIfNull(options);

        return options;
    }
    
    private record AuthenticationOptions(string UserAgent, string Cookie);
}
