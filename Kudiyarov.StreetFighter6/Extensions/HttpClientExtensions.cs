using System.Net;

namespace Kudiyarov.StreetFighter6.Extensions;

public static class HttpClientExtensions
{
    public static void AddStreetFighterClient(this HostApplicationBuilder builder)
    {
        var options = builder.Configuration
            .GetSection("Authentication")
            .Get<AuthenticationOptions>();
        
        ArgumentNullException.ThrowIfNull(options);

        var handler = GetHttpClientHandler(options.Cookie);

        builder.Services
            .AddHttpClient<StreetFighterClient>(client =>
            {
                var userAgent = options.UserAgent;
                client.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);
            })
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                var cookie = options.Cookie;
                return GetHttpClientHandler(cookie);
            })
            .AddStandardResilienceHandler();
    }

    private static HttpClientHandler GetHttpClientHandler(string cookie)
    {
        var handler = new HttpClientHandler();
        var cookies = GetCookies(cookie);
        handler.CookieContainer.Add(cookies);
        return handler;
    }

    private static CookieCollection GetCookies(string cookie)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(cookie);
        
        var rawCookies = cookie.Split(';', StringSplitOptions.TrimEntries);
        var cookies = GetCookies(rawCookies);
        return cookies;
    }

    private static CookieCollection GetCookies(string[] rawCookies)
    {
        var cookies = new CookieCollection();
        
        foreach (var rawCookie in rawCookies)
        {
            var cookie = GetCookie(rawCookie);
            cookies.Add(cookie);
        }

        return cookies;
    }

    private static Cookie GetCookie(string rawCookie)
    {
        var cookieParts = rawCookie.Split('=');

        var name = cookieParts[0];
        var value = cookieParts[1];

        var cookie = new Cookie(name, value, "/", "www.streetfighter.com");
        return cookie;
    }
}

public record AuthenticationOptions(string UserAgent, string Cookie);