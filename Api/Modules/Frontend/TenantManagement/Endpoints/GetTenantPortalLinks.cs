using CIPP.Api.Extensions;
using CIPP.Api.Modules.Frontend.TenantManagement.Models;
using CIPP.Api.Modules.Frontend.TenantManagement.Queries;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.Frontend.TenantManagement.Endpoints;

public static class GetTenantPortalLinks {
    public static void MapGetTenantPortalLinks(this RouteGroupBuilder group) {
        group.Internal().MapGet("/tenant-portals/{tenantId:guid}", Handle)
            .WithName("GetTenantPortalLinks")
            .WithSummary("Get tenant portal links")
            .WithDescription("Returns pre-formatted links to all Microsoft 365 admin portals for the specified tenant")
            .WithTags("Frontend");
    }
    
    private static async Task<IResult> Handle(
        IMediator mediator,
        Guid tenantId,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetTenantPortalLinksQuery(tenantId);
            var links = await mediator.Send(query, cancellationToken);
            
            return Results.Ok(Response<PortalLinks>.SuccessResult(links, "Portal links retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving portal links"
            );
        }
    }
}
