using Microsoft.Net.Http.Headers;

namespace Kudiyarov.StreetFighter6.Extensions;

public static class HttpClientExtensions
{
    public static void AddStreetFighterClient(this HostApplicationBuilder builder)
    {
        var options = builder.Configuration
            .GetSection("Authentication")
            .Get<AuthenticationOptions>();
        
        ArgumentNullException.ThrowIfNull(options);

        builder.Services
            .AddHttpClient<StreetFighterClient>(client =>
            {
                client.DefaultRequestHeaders.Add(HeaderNames.UserAgent, options.UserAgent);
                client.DefaultRequestHeaders.Add(HeaderNames.Cookie, options.Cookie);
            })
            .AddStandardResilienceHandler();
    }
}

public record AuthenticationOptions(string UserAgent, string Cookie);