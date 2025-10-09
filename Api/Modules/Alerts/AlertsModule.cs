using CIPP.Api.Modules.Alerts.Endpoints;
using CIPP.Api.Modules.Alerts.Interfaces;
using CIPP.Api.Modules.Alerts.Services;
using DispatchR.Extensions;
using System.Reflection;

namespace CIPP.Api.Modules.Alerts;

public class AlertsModule {
    public void RegisterServices(IServiceCollection services, IConfiguration configuration) {
        // Register DispatchR
        services.AddDispatchR(Assembly.GetExecutingAssembly(), withPipelines: true, withNotifications: true);
        
        // Register services
        services.AddScoped<IAlertCacheService, AlertCacheService>();
        services.AddScoped<IAlertConfigurationService, AlertConfigurationService>();
        services.AddScoped<IAlertJobService, AlertJobService>();
        services.AddScoped<AlertExecutionService>();
    }

    public void ConfigureEndpoints(RouteGroupBuilder group) {
        group.MapGetAlertConfigurations();
        group.MapCreateAuditLogAlert();
        group.MapCreateScriptedAlert();
        group.MapRemoveAlert();
    }
}
