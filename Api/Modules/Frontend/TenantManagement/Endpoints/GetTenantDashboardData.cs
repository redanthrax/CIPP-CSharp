using CIPP.Api.Extensions;
using CIPP.Api.Modules.Frontend.TenantManagement.Models;
using CIPP.Api.Modules.Frontend.TenantManagement.Queries;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.Frontend.TenantManagement.Endpoints;

public static class GetTenantDashboardData {
    public static void MapGetTenantDashboardData(this RouteGroupBuilder group) {
        group.Internal().MapGet("/tenant-dashboard/{tenantId:guid}", Handle)
            .WithName("GetTenantDashboardData")
            .WithSummary("Get tenant dashboard data")
            .WithDescription("Returns comprehensive dashboard data for a specific tenant, including health status, user counts, license utilization, alerts, and portal links")
            .WithTags("Frontend");
    }
    
    private static async Task<IResult> Handle(
        IMediator mediator,
        Guid tenantId,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetTenantDashboardDataQuery(tenantId);
            var data = await mediator.Send(query, cancellationToken);
            
            return Results.Ok(Response<TenantDashboardData>.SuccessResult(data, "Dashboard data retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving dashboard data"
            );
        }
    }
}
