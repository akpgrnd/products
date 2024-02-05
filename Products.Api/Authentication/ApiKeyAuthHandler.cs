using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Products.Module;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Products.Api.Auth;

public class ApiKeyAuthHandler(
    IUserRepository userRepo,
    IOptionsMonitor<AuthenticationSchemeOptions> options, 
    ILoggerFactory logger, 
    UrlEncoder encoder) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var key = Context.Request.Headers.Authorization;

        if (string.IsNullOrEmpty(key))
        {
            return AuthenticateResult.Fail("Api Key not provided.");
        }

        if (!Guid.TryParse(key, out var apiKey))
        {
            return AuthenticateResult.Fail("Api Key invalid.");
        }

        var user = await userRepo.GetUserByApiKey(apiKey);
        if (user == null) 
        {
            return AuthenticateResult.Fail("");
        }

        var claims = new[] 
        { 
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, "RetrieveProducts") 
        };

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var authTicket = new AuthenticationTicket(new ClaimsPrincipal(identity), Scheme.Name);

        return AuthenticateResult.Success(authTicket);
    }
}
