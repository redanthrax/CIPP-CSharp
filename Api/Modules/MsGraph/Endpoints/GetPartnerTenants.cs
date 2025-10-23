using CIPP.Api.Modules.MsGraph.Interfaces;
using CIPP.Shared.DTOs;
using Microsoft.Graph.Beta.Models;

namespace CIPP.Api.Modules.MsGraph.Endpoints;

public static class GetPartnerTenants {
    public static void MapGetPartnerTenants(this RouteGroupBuilder group) {
        group.MapGet("", Handle)
            .WithName("GetPartnerTenants")
            .WithSummary("Get partner tenants")
            .WithDescription("Retrieves all partner/customer tenant relationships from Microsoft Graph")
            .RequireAuthorization("CombinedAuth");
    }

    private static async Task<IResult> Handle(
        IMicrosoftGraphService graphService,
        string? filter = null,
        int? top = null,
        int? skip = null,
        CancellationToken cancellationToken = default) {
        try {
            var tenants = await graphService.GetPartnerTenantsAsync(filter, top, skip);
            return Results.Ok(Response<ContractCollectionResponse?>.SuccessResult(tenants, "Partner tenants retrieved successfully"));
        } catch (Exception ex) {
            return Results.Json(
                Response<ContractCollectionResponse?>.ErrorResult("Error retrieving partner tenants", new List<string> { ex.Message }),
                statusCode: 500
            );
        }
    }
}
