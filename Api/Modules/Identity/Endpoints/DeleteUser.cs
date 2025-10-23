using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Identity.Commands;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.Identity.Endpoints;

public static class DeleteUser {
    public static void MapDeleteUser(this RouteGroupBuilder group) {
        group.MapDelete("/{id}", Handle)
            .WithName("DeleteUser")
            .WithSummary("Delete user")
            .WithDescription("Deletes a user from the specified tenant")
            .RequirePermission("Identity.User.ReadWrite", "Delete users");
    }

    private static async Task<IResult> Handle(
        string id,
        string tenantId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new DeleteUserCommand(tenantId, id);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<object>.SuccessResult(new object(), "User deleted successfully"));
        } catch (InvalidOperationException ex) {
            return Results.BadRequest(Response<object>.ErrorResult(ex.Message));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error deleting user"
            );
        }
    }
}