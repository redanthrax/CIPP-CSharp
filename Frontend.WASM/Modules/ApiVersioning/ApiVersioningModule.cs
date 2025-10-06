using CIPP.Frontend.WASM.Modules.ApiVersioning.Interfaces;
using CIPP.Frontend.WASM.Modules.ApiVersioning.Services;

namespace CIPP.Frontend.WASM.Modules.ApiVersioning;

public static class ApiVersioningModule {
    public static IServiceCollection AddApiVersioningModule(this IServiceCollection services) {
        services.AddScoped<IApiVersionService, ApiVersionService>();
        
        return services;
    }
}