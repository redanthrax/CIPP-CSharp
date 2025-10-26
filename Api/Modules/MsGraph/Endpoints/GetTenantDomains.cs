using CIPP.Api.Modules.MsGraph.Interfaces;
using CIPP.Shared.DTOs;
using Microsoft.Graph.Beta.Models;

namespace CIPP.Api.Modules.MsGraph.Endpoints;

public static class GetTenantDomains {
    public static void MapGetTenantDomains(this RouteGroupBuilder group) {
        group.MapGet("/{tenantId}/domains", Handle)
            .WithName("GetTenantDomains")
            .WithSummary("Get domains for a tenant")
            .WithDescription("Retrieves all domains for a specific tenant from Microsoft Graph")
            .RequireAuthorization("CombinedAuth");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        IMicrosoftGraphService graphService,
        string? filter = null,
        CancellationToken cancellationToken = default) {
        try {
            var domains = await graphService.GetTenantDomainsAsync(tenantId, filter);
            return Results.Ok(Response<DomainCollectionResponse?>.SuccessResult(domains, "Tenant domains retrieved successfully"));
        } catch (Exception ex) {
            return Results.Json(
                Response<DomainCollectionResponse?>.ErrorResult("Error retrieving tenant domains", new List<string> { ex.Message }),
                statusCode: 500
            );
        }
    }
}
