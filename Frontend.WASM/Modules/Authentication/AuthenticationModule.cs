using CIPP.Frontend.WASM.Modules.Authentication.Handlers;
using CIPP.Frontend.WASM.Modules.Authentication.Interfaces;
using CIPP.Frontend.WASM.Modules.Authentication.Services;
using CIPP.Frontend.WASM.Services;

namespace CIPP.Frontend.WASM.Modules.Authentication;

public static class AuthenticationModule {
    public static IServiceCollection AddAuthenticationModule(this IServiceCollection services, IConfiguration configuration) {
        services.AddMsalAuthentication(options => {
            configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
            var scopes = configuration.GetSection("Api:Scopes").Get<string[]>();
            if (scopes != null) {
                foreach (var scope in scopes) {
                    options.ProviderOptions.DefaultAccessTokenScopes.Add(scope);
                }
            }
        });

        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<UnauthorizedResponseHandler>();
        
        services.AddHttpClient<ICippApiClient, CippApiClient>((serviceProvider, client) => {
            var config = serviceProvider.GetRequiredService<IConfiguration>();
            var baseUrl = config.GetValue<string>("Api:BaseUrl") ?? "https://localhost:8000/api";
            if (!baseUrl.EndsWith("/api") && !baseUrl.EndsWith("/api/")) {
                baseUrl = baseUrl.TrimEnd('/') + "/api";
            }
            
            client.BaseAddress = new Uri(baseUrl);
        }).AddHttpMessageHandler<UnauthorizedResponseHandler>();

        return services;
    }
}