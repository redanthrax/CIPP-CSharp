using CIPP.Frontend.Modules.ApiVersioning.Interfaces;
using CIPP.Frontend.Modules.ApiVersioning.Services;

namespace CIPP.Frontend.Modules.ApiVersioning;

public static class ApiVersioningModule {
    public static IServiceCollection AddApiVersioningModule(this IServiceCollection services) {
        services.AddScoped<IApiVersionService, ApiVersionService>();
        
        return services;
    }
}
