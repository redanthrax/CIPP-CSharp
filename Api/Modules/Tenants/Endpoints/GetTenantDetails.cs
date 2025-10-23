using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Tenants.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Tenants;
using DispatchR;

namespace CIPP.Api.Modules.Tenants.Endpoints;

public static class GetTenantDetails {
    public static void MapGetTenantDetails(this RouteGroupBuilder group) {
        group.MapGet("/{id:guid}/details", Handle)
            .WithName("GetTenantDetails")
            .WithSummary("Get tenant details")
            .WithDescription("Returns comprehensive tenant information including organization data and group memberships")
            .RequirePermission("Tenant.Read", "View detailed tenant information");
    }

    private static async Task<IResult> Handle(
        Guid id,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetTenantDetailsQuery(id);
            var tenantDetails = await mediator.Send(query, cancellationToken);

            return Results.Ok(Response<TenantDetailsDto>.SuccessResult(tenantDetails, "Tenant details retrieved successfully"));
        } catch (InvalidOperationException) {
            return Results.NotFound(Response<TenantDetailsDto>.ErrorResult($"Tenant with ID {id} not found"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving tenant details"
            );
        }
    }
}