using CIPP.Api.Modules.Applications.Endpoints;
using CIPP.Api.Modules.Applications.Interfaces;
using CIPP.Api.Modules.Applications.Services;
using DispatchR.Extensions;
using System.Reflection;

namespace CIPP.Api.Modules.Applications;

public class ApplicationsModule {
    public void RegisterServices(IServiceCollection services, IConfiguration configuration) {
        services.AddDispatchR(Assembly.GetExecutingAssembly(), withPipelines: true, withNotifications: true);
        
        services.AddScoped<IApplicationService, ApplicationService>();
        services.AddScoped<IServicePrincipalService, ServicePrincipalService>();
        services.AddScoped<IAppConsentService, AppConsentService>();
        services.AddScoped<IAppTemplateService, AppTemplateService>();
        services.AddScoped<IPermissionSetService, PermissionSetService>();
    }

    public void ConfigureEndpoints(RouteGroupBuilder group) {
        var appGroup = group.MapGroup("/applications").WithTags("Applications");
        appGroup.MapGetApplications();
        appGroup.MapGetApplication();
        appGroup.MapUpdateApplication();
        appGroup.MapDeleteApplication();
        appGroup.MapCreateApplicationCredential();
        appGroup.MapDeleteApplicationCredential();
        
        appGroup.MapGetAppTemplates();
        appGroup.MapGetAppTemplate();
        appGroup.MapCreateAppTemplate();
        appGroup.MapUpdateAppTemplate();
        appGroup.MapDeleteAppTemplate();
        
        appGroup.MapGetPermissionSets();
        appGroup.MapGetPermissionSet();
        appGroup.MapCreatePermissionSet();
        appGroup.MapUpdatePermissionSet();
        appGroup.MapDeletePermissionSet();
        
        var spGroup = group.MapGroup("/service-principals").WithTags("Service Principals");
        spGroup.MapGetServicePrincipals();
        spGroup.MapGetServicePrincipal();
        spGroup.MapUpdateServicePrincipal();
        spGroup.MapDeleteServicePrincipal();
        spGroup.MapEnableServicePrincipal();
        spGroup.MapDisableServicePrincipal();
        
        var consentGroup = group.MapGroup("/app-consent").WithTags("App Consent");
        consentGroup.MapGetAppConsentRequests();
    }
}
