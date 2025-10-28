using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Commands.MobileDevices;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.MobileDevices;

public static class RemoveMobileDevice {
    public static void MapRemoveMobileDevice(this RouteGroupBuilder group) {
        group.MapDelete("/{deviceId}", Handle)
            .WithName("RemoveMobileDevice")
            .WithSummary("Remove mobile device")
            .WithDescription("Removes a mobile device partnership")
            .RequirePermission("Exchange.MobileDevice.Write", "Remove mobile device");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string deviceId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new RemoveMobileDeviceCommand(tenantId, deviceId);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<object>.SuccessResult(new { }, "Mobile device removed successfully"));
        } catch (Exception ex) {
            return Results.Problem(detail: ex.Message, statusCode: 500, title: "Error removing mobile device");
        }
    }
}