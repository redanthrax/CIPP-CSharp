using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Intune.Commands;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.Intune.Endpoints;

public static class WipeDevice {
    public static void MapWipeDevice(this RouteGroupBuilder group) {
        group.MapPost("/{deviceId}/wipe", Handle)
            .WithName("WipeDevice")
            .WithSummary("Wipe managed device")
            .WithDescription("Initiates a wipe operation on a managed device")
            .RequirePermission("Endpoint.MEM.ReadWrite", "Wipe device");
    }

    public record WipeDeviceRequest(bool KeepEnrollmentData, bool KeepUserData, bool UseProtectedWipe);

    private static async Task<IResult> Handle(
        Guid tenantId,
        string deviceId,
        WipeDeviceRequest request,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new WipeDeviceCommand(tenantId, deviceId, request.KeepEnrollmentData, request.KeepUserData, request.UseProtectedWipe);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<object?>.SuccessResult(null, "Device wipe requested"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error wiping device"
            );
        }
    }
}
