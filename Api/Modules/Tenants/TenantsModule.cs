using CIPP.Api.Modules.Tenants.Endpoints;
using CIPP.Api.Modules.Tenants.Interfaces;
using CIPP.Api.Modules.Tenants.Services;
using DispatchR.Extensions;
using System.Reflection;
namespace CIPP.Api.Modules.Tenants;
public class TenantsModule
{
    public void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDispatchR(Assembly.GetExecutingAssembly(), withPipelines: true, withNotifications: true);
        services.AddScoped<ITenantCacheService, TenantCacheService>();
    }
    public void ConfigureEndpoints(RouteGroupBuilder moduleGroup)
    {
        moduleGroup.MapGetTenants();
        moduleGroup.MapGetTenantById();
        moduleGroup.MapCreateTenant();
    }
}
