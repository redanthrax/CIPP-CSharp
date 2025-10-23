using CIPP.Frontend.Modules.Identity.Interfaces;
using CIPP.Frontend.Modules.Identity.Services;

namespace CIPP.Frontend.Modules.Identity;

public static class IdentityModule {
    public static IServiceCollection AddIdentityModule(this IServiceCollection services) {
        services.AddScoped<IIdentityUserService, IdentityUserService>();
        return services;
    }
}