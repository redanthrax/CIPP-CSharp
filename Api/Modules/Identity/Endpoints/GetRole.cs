using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Identity.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Identity;
using DispatchR;

namespace CIPP.Api.Modules.Identity.Endpoints;

public static class GetRole {
    public static void MapGetRole(this RouteGroupBuilder group) {
        group.MapGet("/{id}", Handle)
            .WithName("GetRole")
            .WithSummary("Get a role")
            .WithDescription("Retrieves a specific role by ID")
            .RequirePermission("Identity.Role.Read", "View role details");
    }

    private static async Task<IResult> Handle(
        string id,
        Guid tenantId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetRoleQuery(tenantId, id);
            var result = await mediator.Send(query, cancellationToken);

            if (result == null) {
                return Results.NotFound(Response<RoleDto>.ErrorResult("Role not found"));
            }

            return Results.Ok(Response<RoleDto>.SuccessResult(result, "Role retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving role"
            );
        }
    }
}
