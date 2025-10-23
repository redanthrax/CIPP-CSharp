using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Intune.Commands;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.Intune.Endpoints;

public static class DefenderScan {
    public static void MapDefenderScan(this RouteGroupBuilder group) {
        group.MapPost("/{deviceId}/defender-scan", Handle)
            .WithName("DefenderScan")
            .WithSummary("Run Defender scan")
            .WithDescription("Initiates Windows Defender scan on a managed device")
            .RequirePermission("Endpoint.MEM.ReadWrite", "Perform Windows Defender scan");
    }

    public record DefenderScanRequest(bool QuickScan);

    private static async Task<IResult> Handle(
        string tenantId,
        string deviceId,
        DefenderScanRequest request,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new DefenderScanCommand(tenantId, deviceId, request.QuickScan);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<object?>.SuccessResult(null, $"Defender {(request.QuickScan ? "quick" : "full")} scan requested"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error initiating Defender scan"
            );
        }
    }
}
