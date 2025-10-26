using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Intune.Commands;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.Intune.Endpoints;

public static class CleanWindowsDevice {
    public static void MapCleanWindowsDevice(this RouteGroupBuilder group) {
        group.MapPost("/{deviceId}/clean", Handle)
            .WithName("CleanWindowsDevice")
            .WithSummary("Clean Windows device")
            .WithDescription("Initiates Fresh Start on a Windows device")
            .RequirePermission("Endpoint.MEM.ReadWrite", "Clean Windows device");
    }

    public record CleanWindowsDeviceRequest(bool KeepUserData);

    private static async Task<IResult> Handle(
        Guid tenantId,
        string deviceId,
        CleanWindowsDeviceRequest request,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new CleanWindowsDeviceCommand(tenantId, deviceId, request.KeepUserData);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<object?>.SuccessResult(null, "Device clean requested"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error cleaning device"
            );
        }
    }
}
