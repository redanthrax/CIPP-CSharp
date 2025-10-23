using CIPP.Api.Modules.Security.Endpoints;
using CIPP.Api.Modules.Security.Interfaces;
using CIPP.Api.Modules.Security.Services;
using DispatchR.Extensions;
using System.Reflection;

namespace CIPP.Api.Modules.Security;

public class SecurityModule {
    public void RegisterServices(IServiceCollection services, IConfiguration configuration) {
        services.AddDispatchR(Assembly.GetExecutingAssembly(), withPipelines: true, withNotifications: true);
        
        services.AddScoped<ISecurityIncidentService, SecurityIncidentService>();
        services.AddScoped<ISecurityAlertService, SecurityAlertService>();
        services.AddScoped<ISecureScoreService, SecureScoreService>();
    }

    public void ConfigureEndpoints(RouteGroupBuilder group) {
        var incidentsGroup = group.MapGroup("/incidents").WithTags("Security Incidents");
        incidentsGroup.MapGetSecurityIncidents();
        incidentsGroup.MapGetSecurityIncident();
        incidentsGroup.MapUpdateSecurityIncident();
        
        var alertsGroup = group.MapGroup("/alerts").WithTags("Security Alerts");
        alertsGroup.MapGetSecurityAlerts();
        alertsGroup.MapGetSecurityAlert();
        alertsGroup.MapUpdateSecurityAlert();
        
        group.MapGetMdoAlerts();
        
        var secureScoreGroup = group.MapGroup("/secure-score").WithTags("Secure Score");
        secureScoreGroup.MapGetSecureScoreControlProfiles();
        secureScoreGroup.MapGetSecureScoreControlProfile();
        secureScoreGroup.MapUpdateSecureScoreControl();
    }
}
