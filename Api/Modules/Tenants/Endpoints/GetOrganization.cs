using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Tenants.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Tenants;
using DispatchR;

namespace CIPP.Api.Modules.Tenants.Endpoints;

public static class GetOrganization {
    public static void MapGetOrganization(this RouteGroupBuilder group) {
        group.MapGet("/{id:guid}/organization", Handle)
            .WithName("GetOrganization")
            .WithSummary("Get organization information")
            .WithDescription("Returns organization details from Microsoft Graph")
            .RequirePermission("Tenant.Read", "View tenant organization information");
    }

    private static async Task<IResult> Handle(
        Guid id,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetOrganizationQuery(id);
            var organization = await mediator.Send(query, cancellationToken);

            if (organization == null) {
                return Results.NotFound(Response<OrganizationDto?>.ErrorResult($"Organization info for tenant {id} not found"));
            }

            return Results.Ok(Response<OrganizationDto>.SuccessResult(organization, "Organization info retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving organization info"
            );
        }
    }
}
