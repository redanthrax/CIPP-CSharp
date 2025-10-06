using CIPP.Api.Extensions;
using CIPP.Api.Modules.Frontend.TenantManagement.Endpoints;
using CIPP.Api.Modules.Frontend.TenantManagement.Interfaces;
using CIPP.Api.Modules.Frontend.TenantManagement.Services;
using CIPP.Api.Modules.Microsoft.Interfaces;
using CIPP.Api.Modules.Microsoft.Services;
using DispatchR.Extensions;
using System.Reflection;

namespace CIPP.Api.Modules.Frontend;

public class FrontendModule : IInternalModule {
    public void RegisterServices(IServiceCollection services, IConfiguration configuration) {
        services.AddDispatchR(Assembly.GetExecutingAssembly(), withPipelines: true, withNotifications: true);
        
        services.AddScoped<ITenantDashboardService, TenantDashboardService>();
        services.AddScoped<IPortalLinkService, PortalLinkService>();
        services.AddScoped<IMicrosoftGraphService, MicrosoftGraphService>();
        services.AddMemoryCache();
    }
    
    public void ConfigureEndpoints(RouteGroupBuilder moduleGroup) {
        ConfigureTenantManagementEndpoints(moduleGroup);
    }
    
    private static void ConfigureTenantManagementEndpoints(RouteGroupBuilder group) {
        group.MapGetTenantDashboardData();
        group.MapGetTenantPortalLinks();
    }
}