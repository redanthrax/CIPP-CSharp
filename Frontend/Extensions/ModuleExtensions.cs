using CIPP.Frontend.Modules.Authentication;
using CIPP.Frontend.Modules.ApiVersioning;
using CIPP.Frontend.Modules.Identity;
using CIPP.Frontend.Modules.Notifications;
using CIPP.Frontend.Modules.Tenants;
using CIPP.Frontend.Services;
using MudBlazor.Services;

namespace CIPP.Frontend.Extensions;

public static class ModuleExtensions {
    public static IServiceCollection AddApplicationModules(this IServiceCollection services, IConfiguration configuration) {
        services.AddMudServices();
        services.AddScoped<IThemeService, ThemeService>();
        
        services.AddApiVersioningModule();
        services.AddNotificationsModule();
        services.AddAuthenticationModule(configuration);
        services.AddTenantsModule();
        services.AddIdentityModule();
        
        return services;
    }
}
