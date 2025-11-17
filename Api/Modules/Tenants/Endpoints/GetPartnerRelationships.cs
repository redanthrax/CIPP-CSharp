using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Tenants.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Tenants;
using DispatchR;

namespace CIPP.Api.Modules.Tenants.Endpoints;

public static class GetPartnerRelationships {
    public static void MapGetPartnerRelationships(this RouteGroupBuilder group) {
        group.MapGet("/{id:guid}/partner-relationships", Handle)
            .WithName("GetPartnerRelationships")
            .WithSummary("Get partner relationships")
            .WithDescription("Returns cross-tenant access policy partners")
            .RequirePermission("Tenant.Read", "View partner relationships");
    }

    private static async Task<IResult> Handle(
        Guid id,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetPartnerRelationshipsQuery(id);
            var partners = await mediator.Send(query, cancellationToken);

            if (partners == null) {
                return Results.Ok(Response<GraphResultsDto<PartnerRelationshipDto>>.SuccessResult(
                    new GraphResultsDto<PartnerRelationshipDto>(), 
                    "No partner relationships found"));
            }

            return Results.Ok(Response<GraphResultsDto<PartnerRelationshipDto>>.SuccessResult(partners, "Partner relationships retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving partner relationships"
            );
        }
    }
}
