using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Intune.Commands;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.Intune.Endpoints;

public static class DeleteAutopilotDevice {
    public static void MapDeleteAutopilotDevice(this RouteGroupBuilder group) {
        group.MapDelete("/{deviceId}", Handle)
            .WithName("DeleteAutopilotDevice")
            .WithSummary("Delete Autopilot device")
            .WithDescription("Deletes a Windows Autopilot device")
            .RequirePermission("Endpoint.Autopilot.ReadWrite", "Delete Autopilot device");
    }

    private static async Task<IResult> Handle(
        string tenantId,
        string deviceId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new DeleteAutopilotDeviceCommand(tenantId, deviceId);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<object?>.SuccessResult(null, "Autopilot device deleted successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error deleting Autopilot device"
            );
        }
    }
}
