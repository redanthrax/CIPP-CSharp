using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Queries.MobileDevices;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.MobileDevices;

public static class GetMobileDeviceStatistics {
    public static void MapGetMobileDeviceStatistics(this RouteGroupBuilder group) {
        group.MapGet("/{deviceId}/statistics", Handle)
            .WithName("GetMobileDeviceStatistics")
            .WithSummary("Get mobile device statistics")
            .WithDescription("Retrieves statistics for a specific mobile device")
            .RequirePermission("Exchange.MobileDevice.Read", "View mobile device statistics");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string deviceId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetMobileDeviceStatisticsQuery(tenantId, deviceId);
            var result = await mediator.Send(query, cancellationToken);

            if (result == null) {
                return Results.NotFound(Response<MobileDeviceStatisticsDto>.ErrorResult("Mobile device statistics not found"));
            }

            return Results.Ok(Response<MobileDeviceStatisticsDto>.SuccessResult(result, "Mobile device statistics retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(detail: ex.Message, statusCode: 500, title: "Error retrieving mobile device statistics");
        }
    }
}