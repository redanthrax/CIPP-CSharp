using CIPP.Api.Modules.Authorization.Interfaces;
using Microsoft.AspNetCore.Authentication;
namespace CIPP.Api.Modules.Authentication.ApiKey;
public static class ApiKeyAuthExtensions
{
    public static IServiceCollection AddApiKeyAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication()
            .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(
                "ApiKey", 
                options => { });
        return services;
    }
}
