using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Intune.Commands;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.Intune.Endpoints;

public static class SyncAutopilotDevices {
    public static void MapSyncAutopilotDevices(this RouteGroupBuilder group) {
        group.MapPost("/sync", Handle)
            .WithName("SyncAutopilotDevices")
            .WithSummary("Sync Autopilot devices")
            .WithDescription("Triggers a sync of Windows Autopilot devices from hardware vendor")
            .RequirePermission("Endpoint.Autopilot.ReadWrite", "Sync Autopilot devices");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new SyncAutopilotDevicesCommand(tenantId);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<object?>.SuccessResult(null, "Autopilot sync initiated successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error syncing Autopilot devices"
            );
        }
    }
}
