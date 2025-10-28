using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Queries.MobileDevices;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.MobileDevices;

public static class GetMobileDevice {
    public static void MapGetMobileDevice(this RouteGroupBuilder group) {
        group.MapGet("/{deviceId}", Handle)
            .WithName("GetMobileDevice")
            .WithSummary("Get mobile device")
            .WithDescription("Retrieves a specific mobile device")
            .RequirePermission("Exchange.MobileDevice.Read", "View mobile device");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string deviceId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetMobileDeviceQuery(tenantId, deviceId);
            var result = await mediator.Send(query, cancellationToken);

            if (result == null) {
                return Results.NotFound(Response<MobileDeviceDto>.ErrorResult("Mobile device not found"));
            }

            return Results.Ok(Response<MobileDeviceDto>.SuccessResult(result, "Mobile device retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(detail: ex.Message, statusCode: 500, title: "Error retrieving mobile device");
        }
    }
}