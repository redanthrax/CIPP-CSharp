using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Intune.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Intune;
using DispatchR;

namespace CIPP.Api.Modules.Intune.Endpoints;

public static class GetAutopilotDevices {
    public static void MapGetAutopilotDevices(this RouteGroupBuilder group) {
        group.MapGet("/", Handle)
            .WithName("GetAutopilotDevices")
            .WithSummary("Get all Autopilot devices")
            .WithDescription("Retrieves all Windows Autopilot devices for the specified tenant")
            .RequirePermission("Endpoint.Autopilot.Read", "View Autopilot devices");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetAutopilotDevicesQuery(tenantId);
            var result = await mediator.Send(query, cancellationToken);

            return Results.Ok(Response<List<AutopilotDeviceDto>>.SuccessResult(result, "Autopilot devices retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving Autopilot devices"
            );
        }
    }
}
