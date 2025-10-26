using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Identity.Commands;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.Identity.Endpoints;

public static class RemoveRole {
    public static void MapRemoveRole(this RouteGroupBuilder group) {
        group.MapDelete("/{roleId}/users/{userId}", Handle)
            .WithName("RemoveRole")
            .WithSummary("Remove a role")
            .WithDescription("Removes a role from a user")
            .RequirePermission("Identity.Role.ReadWrite", "Remove roles");
    }

    private static async Task<IResult> Handle(
        string roleId,
        string userId,
        Guid tenantId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new RemoveRoleCommand(tenantId, roleId, userId);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<string>.SuccessResult("Role removed successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error removing role"
            );
        }
    }
}
