using CIPP.Frontend.WASM.Modules.Authentication;
using CIPP.Frontend.WASM.Modules.Tenants;
using CIPP.Frontend.WASM.Services;
using Frontnet.WASM.Modules.Tenants;
using MudBlazor.Services;

namespace CIPP.Frontend.WASM.Extensions;

public static class ModuleExtensions {
    public static IServiceCollection AddApplicationModules(this IServiceCollection services, IConfiguration configuration) {
        // Core services
        services.AddMudServices();
        services.AddScoped<IThemeService, ThemeService>();
        
        // Application modules
        services.AddAuthenticationModule(configuration);
        services.AddTenantsModule();
        
        return services;
    }
}