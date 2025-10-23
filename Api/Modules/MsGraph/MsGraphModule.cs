using CIPP.Api.Extensions;
using CIPP.Api.Modules.MsGraph.Endpoints;
using CIPP.Api.Modules.MsGraph.Interfaces;
using CIPP.Api.Modules.MsGraph.Services;
using DispatchR.Extensions;
using System.Reflection;

namespace CIPP.Api.Modules.MsGraph;

public class MsGraphModule : IInternalModule {
    public void RegisterServices(IServiceCollection services, IConfiguration configuration) {
        services.AddDispatchR(Assembly.GetExecutingAssembly(), withPipelines: true, withNotifications: true);
        
        services.AddScoped<CachedGraphRequestHandler>();
        services.AddScoped<IGraphExceptionHandler, GraphExceptionHandler>();
        services.AddScoped<GraphExceptionHandler>();
        services.AddScoped<IMicrosoftGraphService, MicrosoftGraphService>();
        services.AddScoped<MicrosoftGraphService>();
        services.AddScoped<GraphUserService>();
        services.AddScoped<GraphGroupService>();
        services.AddScoped<GraphDeviceService>();
        services.AddScoped<GraphRoleService>();
        services.AddScoped<ILicenseService, LicenseService>();
    }
    public void ConfigureEndpoints(RouteGroupBuilder moduleGroup) {
        moduleGroup.MapTestGraph();
        
        var partnerTenantsGroup = moduleGroup.MapGroup("/partner-tenants").WithTags("Partner Tenants");
        partnerTenantsGroup.MapGetPartnerTenants();
        partnerTenantsGroup.MapGetPartnerTenant();
        
        var tenantsGroup = moduleGroup.MapGroup("/tenants").WithTags("Tenant Domains");
        tenantsGroup.MapGetTenantDomains();
    }
}
