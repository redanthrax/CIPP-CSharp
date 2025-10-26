using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Intune.Commands;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.Intune.Endpoints;

public static class CreateDeviceLogCollection {
    public static void MapCreateDeviceLogCollection(this RouteGroupBuilder group) {
        group.MapPost("/{deviceId}/collect-logs", Handle)
            .WithName("CreateDeviceLogCollection")
            .WithSummary("Collect device logs")
            .WithDescription("Creates a log collection request for a managed device")
            .RequirePermission("Endpoint.MEM.ReadWrite", "Collect device logs");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string deviceId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new CreateDeviceLogCollectionCommand(tenantId, deviceId);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<object?>.SuccessResult(null, "Device log collection requested"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error creating device log collection"
            );
        }
    }
}
