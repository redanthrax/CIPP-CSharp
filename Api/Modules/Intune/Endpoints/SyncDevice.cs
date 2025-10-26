using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Intune.Commands;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.Intune.Endpoints;

public static class SyncDevice {
    public static void MapSyncDevice(this RouteGroupBuilder group) {
        group.MapPost("/{deviceId}/sync", Handle)
            .WithName("SyncDevice")
            .WithSummary("Sync managed device")
            .WithDescription("Initiates sync for a managed device")
            .RequirePermission("Endpoint.MEM.ReadWrite", "Sync device");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string deviceId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new SyncDeviceCommand(tenantId, deviceId);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<object?>.SuccessResult(null, "Device sync requested"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error syncing device"
            );
        }
    }
}
