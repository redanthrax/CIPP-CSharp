using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
namespace CIPP.Api.Modules.Authentication.EntraId;
public static class EntraIdAuthExtensions
{
    public static IServiceCollection AddEntraIdAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(configuration.GetSection("Authentication:AzureAd"));
        services.AddAuthorization(options =>
        {
            options.AddPolicy("EntraIdPolicy", policy =>
                policy.RequireAuthenticatedUser()
                      .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme));
        });
        return services;
    }
}
