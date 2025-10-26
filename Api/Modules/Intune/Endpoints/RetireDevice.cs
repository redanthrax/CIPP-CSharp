using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Intune.Commands;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.Intune.Endpoints;

public static class RetireDevice {
    public static void MapRetireDevice(this RouteGroupBuilder group) {
        group.MapPost("/{deviceId}/retire", Handle)
            .WithName("RetireDevice")
            .WithSummary("Retire managed device")
            .WithDescription("Retires a managed device")
            .RequirePermission("Endpoint.MEM.ReadWrite", "Retire device");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string deviceId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new RetireDeviceCommand(tenantId, deviceId);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<object?>.SuccessResult(null, "Device retire requested"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retiring device"
            );
        }
    }
}
