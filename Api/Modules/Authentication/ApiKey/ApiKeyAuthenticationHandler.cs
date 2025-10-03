using System.Security.Claims;
using System.Text.Encodings.Web;
using CIPP.Api.Modules.Authorization.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
namespace CIPP.Api.Modules.Authentication.ApiKey;
public class ApiKeyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IApiKeyService _apiKeyService;
    public ApiKeyAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, 
        ILoggerFactory logger, UrlEncoder encoder, IApiKeyService apiKeyService) 
        : base(options, logger, encoder)
    {
        _apiKeyService = apiKeyService;
    }
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        const string apiKeyHeader = "X-API-Key";
        if (!Request.Headers.ContainsKey(apiKeyHeader))
        {
            return AuthenticateResult.NoResult();
        }
        var apiKey = Request.Headers[apiKeyHeader].FirstOrDefault();
        if (string.IsNullOrEmpty(apiKey))
        {
            return AuthenticateResult.Fail("Invalid API key");
        }
        var isValid = await _apiKeyService.ValidateApiKeyAsync(apiKey);
        if (!isValid)
        {
            return AuthenticateResult.Fail("Invalid API key");
        }
        await _apiKeyService.RecordApiKeyUsageAsync(apiKey);
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, "API Key"),
            new Claim(ClaimTypes.AuthenticationMethod, "ApiKey"),
            new Claim("cipp:api_key", apiKey)
        };
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);
        return AuthenticateResult.Success(ticket);
    }
}
