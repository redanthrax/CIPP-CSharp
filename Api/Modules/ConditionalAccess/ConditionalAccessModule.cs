using CIPP.Api.Modules.ConditionalAccess.Endpoints;
using CIPP.Api.Modules.ConditionalAccess.Interfaces;
using CIPP.Api.Modules.ConditionalAccess.Services;
using DispatchR.Extensions;
using System.Reflection;

namespace CIPP.Api.Modules.ConditionalAccess;

public class ConditionalAccessModule {
    public void RegisterServices(IServiceCollection services, IConfiguration configuration) {
        services.AddDispatchR(Assembly.GetExecutingAssembly(), withPipelines: true, withNotifications: true);
        
        services.AddScoped<IConditionalAccessPolicyService, ConditionalAccessPolicyService>();
        services.AddScoped<INamedLocationService, NamedLocationService>();
        services.AddScoped<IConditionalAccessTemplateService, ConditionalAccessTemplateService>();
    }

    public void ConfigureEndpoints(RouteGroupBuilder group) {
        group.MapGetConditionalAccessPolicies();
        group.MapGetConditionalAccessPolicy();
        group.MapCreateConditionalAccessPolicy();
        group.MapUpdateConditionalAccessPolicy();
        group.MapDeleteConditionalAccessPolicy();
        
        group.MapGetNamedLocations();
        group.MapGetNamedLocation();
        group.MapCreateNamedLocation();
        group.MapDeleteNamedLocation();
        
        var templatesGroup = group.MapGroup("/templates").WithTags("CA Templates");
        templatesGroup.MapGetConditionalAccessTemplates();
        templatesGroup.MapGetConditionalAccessTemplate();
        templatesGroup.MapCreateConditionalAccessTemplate();
        templatesGroup.MapUpdateConditionalAccessTemplate();
        templatesGroup.MapDeleteConditionalAccessTemplate();
        templatesGroup.MapDeployConditionalAccessTemplate();
    }
}
