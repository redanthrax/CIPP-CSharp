using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Intune.Commands;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.Intune.Endpoints;

public static class DefenderUpdateSignatures {
    public static void MapDefenderUpdateSignatures(this RouteGroupBuilder group) {
        group.MapPost("/{deviceId}/defender-update", Handle)
            .WithName("DefenderUpdateSignatures")
            .WithSummary("Update Defender signatures")
            .WithDescription("Updates Windows Defender signatures on a managed device")
            .RequirePermission("Endpoint.MEM.ReadWrite", "Update Windows Defender signatures");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string deviceId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new DefenderUpdateSignaturesCommand(tenantId, deviceId);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<object?>.SuccessResult(null, "Defender signature update requested"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error updating Defender signatures"
            );
        }
    }
}
