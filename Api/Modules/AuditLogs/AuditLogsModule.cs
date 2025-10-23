using CIPP.Api.Modules.AuditLogs.Endpoints;
using CIPP.Api.Modules.AuditLogs.Interfaces;
using CIPP.Api.Modules.AuditLogs.Services;
using DispatchR.Extensions;
using System.Reflection;

namespace CIPP.Api.Modules.AuditLogs;

public class AuditLogsModule {
    public void RegisterServices(IServiceCollection services, IConfiguration configuration) {
        services.AddDispatchR(Assembly.GetExecutingAssembly(), withPipelines: true, withNotifications: true);
        services.AddScoped<IAuditLogService, AuditLogService>();
        services.AddScoped<IAuditLogSearchService, AuditLogSearchService>();
    }

    public void ConfigureEndpoints(RouteGroupBuilder moduleGroup) {
        moduleGroup.MapGetAuditLogs();
        moduleGroup.MapGetAuditLogSearches();
        moduleGroup.MapGetAuditLogSearchResults();
        moduleGroup.MapCreateAuditLogSearch();
        moduleGroup.MapProcessAuditLogSearch();
    }
}
