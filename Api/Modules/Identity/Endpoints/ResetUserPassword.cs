using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Identity.Commands;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Identity;
using DispatchR;

namespace CIPP.Api.Modules.Identity.Endpoints;

public static class ResetUserPassword {
    public static void MapResetUserPassword(this RouteGroupBuilder group) {
        group.MapPost("/{id}/reset-password", Handle)
            .WithName("ResetUserPassword")
            .WithSummary("Reset user password")
            .WithDescription("Resets a user's password")
            .RequirePermission("Identity.User.ReadWrite", "Reset user passwords");
    }

    private static async Task<IResult> Handle(
        string id,
        ResetUserPasswordDto passwordData,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new ResetUserPasswordCommand(passwordData.TenantId, id, passwordData);
            var result = await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<string>.SuccessResult(result, "User password reset successfully"));
        } catch (InvalidOperationException ex) {
            return Results.BadRequest(Response<string>.ErrorResult(ex.Message));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error resetting user password"
            );
        }
    }
}