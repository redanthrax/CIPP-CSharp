using CIPP.Api.Modules.Authentication.ApiKey;
using CIPP.Api.Modules.Authentication.EntraId;
using Microsoft.AspNetCore.Authentication.JwtBearer;
namespace CIPP.Api.Modules.Authentication;
public class AuthenticationModule
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
        moduleGroup.MapPost("/test-auth", () => "Authentication module loaded")
            .WithName("TestAuth")
            .WithSummary("Test authentication module")
            .RequireAuthorization("CombinedAuth");
    }
}
