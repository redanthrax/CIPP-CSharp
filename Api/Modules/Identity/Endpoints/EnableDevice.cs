using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Identity.Commands;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.Identity.Endpoints;

public static class EnableDevice {
    public static void MapEnableDevice(this RouteGroupBuilder group) {
        group.MapPost("/{id}/enable", Handle)
            .WithName("EnableDevice")
            .WithSummary("Enable a device")
            .WithDescription("Enables a device in the tenant")
            .RequirePermission("Identity.Device.ReadWrite", "Enable devices");
    }

    private static async Task<IResult> Handle(
        string id,
        Guid tenantId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new EnableDeviceCommand(tenantId, id);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<string>.SuccessResult("Device enabled successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error enabling device"
            );
        }
    }
}
