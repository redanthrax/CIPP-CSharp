using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Authorization.Endpoints;
using CIPP.Api.Modules.Authorization.Interfaces;
using CIPP.Api.Modules.Authorization.Services;
using CIPP.Api.Extensions;
using Microsoft.AspNetCore.Authorization;
using System.Reflection;
using DispatchR.Extensions;

namespace CIPP.Api.Modules.Authorization;

public class AuthorizationModule : IInternalModule {
    public void RegisterServices(IServiceCollection services, IConfiguration configuration) {
        services.AddHttpContextAccessor();
        services.AddDispatchR(Assembly.GetExecutingAssembly(), withPipelines: true, withNotifications: true);
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IApiKeyService, ApiKeyService>();
        services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
    }

    public void ConfigureEndpoints(RouteGroupBuilder moduleGroup) {
        moduleGroup.MapCreateApiKey();
        moduleGroup.MapGetApiKeys();
    }

    public static async Task ConfigureAsync(IApplicationBuilder app) {
        using var scope = app.ApplicationServices.CreateScope();
        var permissionService = scope.ServiceProvider.GetRequiredService<IPermissionService>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<AuthorizationModule>>();
        
        try {
            logger.LogInformation("Initializing authorization system...");
            var permissionCount = await permissionService.DiscoverAndRegisterPermissionsAsync();
            logger.LogInformation("Discovered and registered {Count} permissions", permissionCount);
            var superAdminRole = await permissionService.EnsureSuperAdminRoleAsync();
            logger.LogInformation("Super Admin role ensured with ID: {RoleId}", superAdminRole.Id);
            logger.LogInformation("Authorization system initialization complete");
        }
        catch (Exception ex) {
            logger.LogError(ex, "Failed to initialize authorization system");
            throw;
        }
    }
}
