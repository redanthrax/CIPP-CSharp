using CIPP.Api.Modules.MsGraph.Interfaces;
using CIPP.Shared.DTOs;
using Microsoft.Graph.Beta.Models;

namespace CIPP.Api.Modules.MsGraph.Endpoints;

public static class GetPartnerTenant {
    public static void MapGetPartnerTenant(this RouteGroupBuilder group) {
        group.MapGet("/{contractId}", Handle)
            .WithName("GetPartnerTenant")
            .WithSummary("Get partner tenant by contract ID")
            .WithDescription("Retrieves a specific partner tenant by contract ID from Microsoft Graph")
            .RequireAuthorization("CombinedAuth");
    }

    private static async Task<IResult> Handle(
        string contractId,
        IMicrosoftGraphService graphService,
        CancellationToken cancellationToken = default) {
        try {
            var tenant = await graphService.GetPartnerTenantAsync(contractId);
            
            if (tenant == null) {
                return Results.NotFound(Response<Contract?>.ErrorResult("Partner tenant not found", new List<string> { $"Contract {contractId} not found" }));
            }

            return Results.Ok(Response<Contract?>.SuccessResult(tenant, "Partner tenant retrieved successfully"));
        } catch (Exception ex) {
            return Results.Json(
                Response<Contract?>.ErrorResult("Error retrieving partner tenant", new List<string> { ex.Message }),
                statusCode: 500
            );
        }
    }
}
