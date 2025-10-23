using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Intune.Commands;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.Intune.Endpoints;

public static class ResetPasscode {
    public static void MapResetPasscode(this RouteGroupBuilder group) {
        group.MapPost("/{deviceId}/reset-passcode", Handle)
            .WithName("ResetPasscode")
            .WithSummary("Reset device passcode")
            .WithDescription("Resets the passcode on a managed device")
            .RequirePermission("Endpoint.MEM.ReadWrite", "Reset device passcode");
    }

    private static async Task<IResult> Handle(
        string tenantId,
        string deviceId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new ResetPasscodeCommand(tenantId, deviceId);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<object?>.SuccessResult(null, "Passcode reset requested"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error resetting passcode"
            );
        }
    }
}
