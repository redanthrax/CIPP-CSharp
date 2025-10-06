using CIPP.Api.Modules.Tenants.Queries;
using CIPP.Api.Modules.Tenants.Models;
using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.Tenants.Endpoints;

public static class GetTenantCapabilities {
    public static void MapGetTenantCapabilities(this RouteGroupBuilder group) {
        group.MapGet("/{tenantId:guid}/capabilities", HandleAsync)
            .WithName("GetTenantCapabilities")
            .WithSummary("Get tenant service capabilities")
            .WithDescription("Returns what Microsoft 365 services are available for the tenant")
            .RequirePermission("Tenant.Read", "View tenant capabilities and service information");
    }
    
    private static async Task<IResult> HandleAsync(
        IMediator mediator,
        Guid tenantId,
        CancellationToken cancellationToken = default) {
        var query = new GetTenantCapabilitiesQuery(tenantId);
        var capabilities = await mediator.Send(query, cancellationToken);
        
        return capabilities != null
            ? Results.Ok(Response<TenantCapabilities>.SuccessResult(capabilities))
            : Results.NotFound(Response<TenantCapabilities>.ErrorResult($"Tenant {tenantId} not found or has no capabilities configured"));
    }
}