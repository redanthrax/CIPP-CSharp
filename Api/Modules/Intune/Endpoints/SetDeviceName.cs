using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Intune.Commands;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.Intune.Endpoints;

public static class SetDeviceName {
    public static void MapSetDeviceName(this RouteGroupBuilder group) {
        group.MapPost("/{deviceId}/set-name", Handle)
            .WithName("SetDeviceName")
            .WithSummary("Set device name")
            .WithDescription("Sets the name of a managed device")
            .RequirePermission("Endpoint.MEM.ReadWrite", "Set device name");
    }

    public record SetDeviceNameRequest(string DeviceName);

    private static async Task<IResult> Handle(
        string tenantId,
        string deviceId,
        SetDeviceNameRequest request,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new SetDeviceNameCommand(tenantId, deviceId, request.DeviceName);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<object?>.SuccessResult(null, "Device name set requested"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error setting device name"
            );
        }
    }
}
