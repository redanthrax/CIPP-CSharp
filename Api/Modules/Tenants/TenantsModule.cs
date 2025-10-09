using CIPP.Api.Modules.Tenants.Endpoints;
using CIPP.Api.Modules.Tenants.Interfaces;
using CIPP.Api.Modules.Tenants.Services;
using CIPP.Api.Modules.Microsoft.Services;
using DispatchR.Extensions;
using System.Reflection;
using CIPP.Api.Modules.Microsoft.Interfaces;

namespace CIPP.Api.Modules.Tenants;

public class TenantsModule
{
    public void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDispatchR(Assembly.GetExecutingAssembly(), withPipelines: true, withNotifications: true);
        services.AddScoped<ITenantCacheService, TenantCacheService>();
        services.AddScoped<IMicrosoftGraphService, MicrosoftGraphService>();
    }
    
    public void ConfigureEndpoints(RouteGroupBuilder moduleGroup)
    {
        moduleGroup.MapGetTenants();
        moduleGroup.MapGetTenantById();
        moduleGroup.MapGetTenantDetails();
        moduleGroup.MapCreateTenant();
        moduleGroup.MapUpdateTenant();
        moduleGroup.MapExcludeTenant();
        moduleGroup.MapGetTenantGroups();
        moduleGroup.MapGetTenantGroup();
        moduleGroup.MapCreateTenantGroup();
        moduleGroup.MapUpdateTenantGroup();
        moduleGroup.MapDeleteTenantGroup();
        moduleGroup.MapGetTenantCapabilities();
        moduleGroup.MapSyncTenantFromGraph();
        moduleGroup.MapValidateDomain();
    }
}
