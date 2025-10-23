using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Identity.Commands;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Identity;
using DispatchR;

namespace CIPP.Api.Modules.Identity.Endpoints;

public static class UpdateUser {
    public static void MapUpdateUser(this RouteGroupBuilder group) {
        group.MapPut("/{id}", Handle)
            .WithName("UpdateUser")
            .WithSummary("Update user")
            .WithDescription("Updates an existing user")
            .RequirePermission("Identity.User.ReadWrite", "Update users");
    }

    private static async Task<IResult> Handle(
        string id,
        string tenantId,
        UpdateUserDto userData,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new UpdateUserCommand(tenantId, id, userData);
            var result = await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<UserDto>.SuccessResult(result, "User updated successfully"));
        } catch (InvalidOperationException ex) {
            return Results.BadRequest(Response<UserDto>.ErrorResult(ex.Message));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error updating user"
            );
        }
    }
}