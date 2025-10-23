using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Intune.Commands;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.Intune.Endpoints;

public static class RemoteLockDevice {
    public static void MapRemoteLockDevice(this RouteGroupBuilder group) {
        group.MapPost("/{deviceId}/remote-lock", Handle)
            .WithName("RemoteLockDevice")
            .WithSummary("Remote lock device")
            .WithDescription("Locks a managed device remotely")
            .RequirePermission("Endpoint.MEM.ReadWrite", "Remote lock device");
    }

    private static async Task<IResult> Handle(
        string tenantId,
        string deviceId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new RemoteLockCommand(tenantId, deviceId);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<object?>.SuccessResult(null, "Device remote lock requested"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error remote locking device"
            );
        }
    }
}
