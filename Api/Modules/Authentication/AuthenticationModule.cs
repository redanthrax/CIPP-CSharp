using CIPP.Api.Modules.Authentication.ApiKey;
using CIPP.Api.Modules.Authentication.EntraId;
using CIPP.Api.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
namespace CIPP.Api.Modules.Authentication;

public class AuthenticationModule : IInternalModule
{
    public void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddEntraIdAuthentication(configuration);
        services.AddApiKeyAuthentication();
        services.AddAuthorization(options =>
        {
            options.AddPolicy("CombinedAuth", policy =>
            {
                policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme, "ApiKey")
                      .RequireAuthenticatedUser();
            });
        });
    }
    public void ConfigureEndpoints(RouteGroupBuilder moduleGroup)
    {
        // No endpoints to configure for authentication module
    }
}
