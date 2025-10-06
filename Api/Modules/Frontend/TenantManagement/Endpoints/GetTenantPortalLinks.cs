using CIPP.Api.Extensions;
using CIPP.Api.Modules.Frontend.TenantManagement.Models;
using CIPP.Api.Modules.Frontend.TenantManagement.Queries;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.Frontend.TenantManagement.Endpoints;

public static class GetTenantPortalLinks {
    public static void MapGetTenantPortalLinks(this RouteGroupBuilder group) {
        group.Internal().MapGet("/tenant-portals/{tenantId:guid}", HandleAsync)
            .WithName("GetTenantPortalLinks")
            .WithSummary("Get tenant portal links for CIPP frontend")
            .WithDescription("Returns pre-formatted links to all Microsoft 365 admin portals for the specified tenant");
    }
    
    private static async Task<IResult> HandleAsync(
        IMediator mediator,
        Guid tenantId,
        CancellationToken cancellationToken = default) {
        var query = new GetTenantPortalLinksQuery(tenantId);
        var links = await mediator.Send(query, cancellationToken);
        
        return Results.Ok(Response<PortalLinks>.SuccessResult(links));
    }
}