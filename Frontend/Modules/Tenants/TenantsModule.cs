using CIPP.Frontend.Modules.Tenants.Interfaces;
using CIPP.Frontend.Modules.Tenants.Services;

namespace CIPP.Frontend.Modules.Tenants;

public static class TenantsModule {
    public static IServiceCollection AddTenantsModule(this IServiceCollection services)
    {
        services.AddScoped<ITenantService, TenantService>();
        return services;
    }
}
