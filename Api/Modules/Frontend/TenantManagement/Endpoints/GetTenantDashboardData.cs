using CIPP.Api.Extensions;
using CIPP.Api.Modules.Frontend.TenantManagement.Models;
using CIPP.Api.Modules.Frontend.TenantManagement.Queries;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.Frontend.TenantManagement.Endpoints;

public static class GetTenantDashboardData {
    public static void MapGetTenantDashboardData(this RouteGroupBuilder group) {
        group.Internal().MapGet("/tenant-dashboard/{tenantId:guid}", HandleAsync)
            .WithName("GetTenantDashboardData")
            .WithSummary("Get tenant dashboard data for CIPP frontend")
            .WithDescription("Returns comprehensive dashboard data for a specific tenant, including health status, user counts, license utilization, alerts, and portal links");
    }
    
    private static async Task<IResult> HandleAsync(
        IMediator mediator,
        Guid tenantId,
        CancellationToken cancellationToken = default) {
        var query = new GetTenantDashboardDataQuery(tenantId);
        var data = await mediator.Send(query, cancellationToken);
        
        return Results.Ok(Response<TenantDashboardData>.SuccessResult(data));
    }
}