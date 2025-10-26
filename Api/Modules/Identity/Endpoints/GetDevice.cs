using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Identity.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Identity;
using DispatchR;

namespace CIPP.Api.Modules.Identity.Endpoints;

public static class GetDevice {
    public static void MapGetDevice(this RouteGroupBuilder group) {
        group.MapGet("/{id}", Handle)
            .WithName("GetDevice")
            .WithSummary("Get a device")
            .WithDescription("Retrieves a specific device by ID")
            .RequirePermission("Identity.Device.Read", "View device details");
    }

    private static async Task<IResult> Handle(
        string id,
        Guid tenantId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetDeviceQuery(tenantId, id);
            var result = await mediator.Send(query, cancellationToken);

            if (result == null) {
                return Results.NotFound(Response<DeviceDto>.ErrorResult("Device not found"));
            }

            return Results.Ok(Response<DeviceDto>.SuccessResult(result, "Device retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving device"
            );
        }
    }
}
