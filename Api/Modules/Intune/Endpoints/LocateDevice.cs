using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Intune.Commands;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.Intune.Endpoints;

public static class LocateDevice {
    public static void MapLocateDevice(this RouteGroupBuilder group) {
        group.MapPost("/{deviceId}/locate", Handle)
            .WithName("LocateDevice")
            .WithSummary("Locate managed device")
            .WithDescription("Initiates device location for a managed device")
            .RequirePermission("Endpoint.MEM.ReadWrite", "Locate device");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string deviceId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new LocateDeviceCommand(tenantId, deviceId);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<object?>.SuccessResult(null, "Device locate requested"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error locating device"
            );
        }
    }
}
