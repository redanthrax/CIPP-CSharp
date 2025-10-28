using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Commands.MobileDevices;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.MobileDevices;

public static class ClearMobileDevice {
    public static void MapClearMobileDevice(this RouteGroupBuilder group) {
        group.MapPost("/{deviceId}/clear", Handle)
            .WithName("ClearMobileDevice")
            .WithSummary("Clear/wipe mobile device")
            .WithDescription("Initiates or cancels a remote wipe of a mobile device")
            .RequirePermission("Exchange.MobileDevice.Write", "Clear mobile device");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string deviceId,
        ClearMobileDeviceDto clearDto,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new ClearMobileDeviceCommand(tenantId, deviceId, clearDto);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<object>.SuccessResult(new { }, 
                clearDto.Cancel ? "Mobile device wipe cancelled successfully" : "Mobile device wipe initiated successfully"));
        } catch (Exception ex) {
            return Results.Problem(detail: ex.Message, statusCode: 500, title: "Error clearing mobile device");
        }
    }
}