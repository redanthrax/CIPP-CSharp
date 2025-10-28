using CIPP.Api.Modules.Standards.Analyzers;
using CIPP.Api.Modules.Standards.Endpoints;
using CIPP.Api.Modules.Standards.Executors;
using CIPP.Api.Modules.Standards.Interfaces;
using CIPP.Api.Modules.Standards.Services;
using DispatchR.Extensions;
using System.Reflection;

namespace CIPP.Api.Modules.Standards;

public class StandardsModule {
    public void RegisterServices(IServiceCollection services, IConfiguration configuration) {
        services.AddDispatchR(Assembly.GetExecutingAssembly(), withPipelines: true, withNotifications: true);
        services.AddScoped<IStandardsService, StandardsService>();
        services.AddSingleton<StandardExecutorFactory>();
        services.AddScoped<ConditionalAccessStandardExecutor>();
        services.AddScoped<ExchangeStandardExecutor>();
        services.AddScoped<IntuneStandardExecutor>();
        services.AddScoped<SecurityStandardExecutor>();
        services.AddScoped<IdentityStandardExecutor>();
        services.AddScoped<SharePointStandardExecutor>();
        services.AddScoped<TeamsStandardExecutor>();
        
        services.AddScoped<IBpaService, BpaService>();
        services.AddScoped<IBpaAnalyzer, SecurityBpaAnalyzer>();
        services.AddScoped<IBpaAnalyzer, IdentityBpaAnalyzer>();
        services.AddScoped<IBpaAnalyzer, ExchangeBpaAnalyzer>();
        services.AddScoped<IBpaAnalyzer, ConditionalAccessBpaAnalyzer>();
        services.AddScoped<IBpaAnalyzer, IntuneBpaAnalyzer>();
    }

    public void ConfigureEndpoints(RouteGroupBuilder group) {
        var standardsGroup = group.MapGroup("/standards").WithTags("Standards");
        
        standardsGroup.MapGetStandardTemplates();
        standardsGroup.MapGetStandardTemplate();
        standardsGroup.MapCreateStandardTemplate();
        standardsGroup.MapUpdateStandardTemplate();
        standardsGroup.MapDeleteStandardTemplate();
        standardsGroup.MapDeployStandard();
        standardsGroup.MapGetExecutionHistory();
        
        standardsGroup.MapGetBpaReport();
        standardsGroup.MapGetBpaRecommendations();
        standardsGroup.MapGetComplianceScore();
    }
}
