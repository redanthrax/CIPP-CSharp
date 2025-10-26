using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Intune.Commands;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.Intune.Endpoints;

public static class RebootDevice {
    public static void MapRebootDevice(this RouteGroupBuilder group) {
        group.MapPost("/{deviceId}/reboot", Handle)
            .WithName("RebootDevice")
            .WithSummary("Reboot managed device")
            .WithDescription("Initiates reboot for a managed device")
            .RequirePermission("Endpoint.MEM.ReadWrite", "Reboot device");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string deviceId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new RebootDeviceCommand(tenantId, deviceId);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<object?>.SuccessResult(null, "Device reboot requested"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error rebooting device"
            );
        }
    }
}
