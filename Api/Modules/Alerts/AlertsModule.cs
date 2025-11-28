using CIPP.Api.Modules.Alerts.Endpoints;
using CIPP.Api.Modules.Alerts.Interfaces;
using CIPP.Api.Modules.Alerts.Services;
using DispatchR.Extensions;
using System.Reflection;

namespace CIPP.Api.Modules.Alerts;

public class AlertsModule {
    public void RegisterServices(IServiceCollection services, IConfiguration configuration) {
        services.AddDispatchR(Assembly.GetExecutingAssembly(), withPipelines: true, withNotifications: true);
        
        services.AddScoped<IAlertCacheService, AlertCacheService>();
        services.AddScoped<IAlertConfigurationService, AlertConfigurationService>();
        services.AddScoped<IAlertJobService, AlertJobService>();
        services.AddScoped<IGraphWebhookService, GraphWebhookService>();
        services.AddScoped<IAuditLogProcessor, AuditLogProcessor>();
        services.AddScoped<IAlertTemplateService, AlertTemplateService>();
        services.AddScoped<AlertExecutionService>();
        services.AddScoped<AuditDataEnricher>();
        services.AddScoped<AlertRuleEvaluator>();
        
        services.AddScoped<IAlertActionHandler, EmailAlertActionHandler>();
        services.AddScoped<IAlertActionHandler, WebhookAlertActionHandler>();
        services.AddScoped<IAlertActionHandler, DisableUserAlertActionHandler>();
        services.AddScoped<IAlertActionHandler, BecRemediationAlertActionHandler>();
        
        services.AddHttpClient();
        
        services.AddHostedService<WebhookProcessorHostedService>();
        services.AddHostedService<SubscriptionRenewalHostedService>();
    }

    public void ConfigureEndpoints(RouteGroupBuilder group) {
        group.MapGetAlertConfigurations();
        group.MapCreateAuditLogAlert();
        group.MapCreateScriptedAlert();
        group.MapRemoveAlert();
        group.MapReceiveGraphWebhook();
        group.MapGetAlertHistory();
        group.MapGetAlertStatistics();
    }
}
