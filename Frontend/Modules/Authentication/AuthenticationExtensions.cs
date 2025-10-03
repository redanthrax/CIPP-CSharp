using CIPP.Frontend.Modules.Authentication.Interfaces;
using CIPP.Frontend.Modules.Authentication.Services;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

namespace CIPP.Frontend.Modules.Authentication;

public static class AuthenticationExtensions {
    public static IServiceCollection AddCippAuthentication(this IServiceCollection services, IConfiguration configuration) {
        services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApp(options => {
                configuration.Bind("EntraId", options);
                options.Events.OnRedirectToIdentityProvider = context => {
                    return Task.CompletedTask;
                };
            })
            .EnableTokenAcquisitionToCallDownstreamApi()
            .AddInMemoryTokenCaches();

        services.AddAuthorization();
        services.AddControllers()
            .AddMicrosoftIdentityUI();

        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddHttpClient<ICippApiClient, CippApiClient>((serviceProvider, client) => {
            var config = serviceProvider.GetRequiredService<IConfiguration>();
            var baseUrl = config.GetValue<string>("DownstreamApi:BaseUrl") ?? "https://localhost:8000/api";
            if (!baseUrl.EndsWith("/api") && !baseUrl.EndsWith("/api/")) {
                baseUrl = baseUrl.TrimEnd('/') + "/api";
            }
            
            client.BaseAddress = new Uri(baseUrl);
        });

        return services;
    }
}