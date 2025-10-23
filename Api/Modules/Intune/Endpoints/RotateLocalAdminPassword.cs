using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Intune.Commands;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.Intune.Endpoints;

public static class RotateLocalAdminPassword {
    public static void MapRotateLocalAdminPassword(this RouteGroupBuilder group) {
        group.MapPost("/{deviceId}/rotate-laps", Handle)
            .WithName("RotateLocalAdminPassword")
            .WithSummary("Rotate local admin password")
            .WithDescription("Rotates the local administrator password (LAPS) on a managed device")
            .RequirePermission("Endpoint.MEM.ReadWrite", "Rotate local admin password");
    }

    private static async Task<IResult> Handle(
        string tenantId,
        string deviceId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new RotateLocalAdminPasswordCommand(tenantId, deviceId);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<object?>.SuccessResult(null, "Local admin password rotation requested"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error rotating local admin password"
            );
        }
    }
}
