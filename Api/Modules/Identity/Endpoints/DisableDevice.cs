using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Identity.Commands;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.Identity.Endpoints;

public static class DisableDevice {
    public static void MapDisableDevice(this RouteGroupBuilder group) {
        group.MapPost("/{id}/disable", Handle)
            .WithName("DisableDevice")
            .WithSummary("Disable a device")
            .WithDescription("Disables a device in the tenant")
            .RequirePermission("Identity.Device.ReadWrite", "Disable devices");
    }

    private static async Task<IResult> Handle(
        string id,
        Guid tenantId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new DisableDeviceCommand(tenantId, id);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<string>.SuccessResult("Device disabled successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error disabling device"
            );
        }
    }
}
