using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Identity.Commands;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.Identity.Endpoints;

public static class DeleteDevice {
    public static void MapDeleteDevice(this RouteGroupBuilder group) {
        group.MapDelete("/{id}", Handle)
            .WithName("DeleteDevice")
            .WithSummary("Delete a device")
            .WithDescription("Deletes a device from the tenant")
            .RequirePermission("Identity.Device.ReadWrite", "Delete devices");
    }

    private static async Task<IResult> Handle(
        string id,
        Guid tenantId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new DeleteDeviceCommand(tenantId, id);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<string>.SuccessResult("Device deleted successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error deleting device"
            );
        }
    }
}
