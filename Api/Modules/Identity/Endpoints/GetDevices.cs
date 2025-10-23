using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Identity.Queries;
using CIPP.Api.Extensions;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Identity;
using DispatchR;

namespace CIPP.Api.Modules.Identity.Endpoints;

public static class GetDevices {
    public static void MapGetDevices(this RouteGroupBuilder group) {
        group.MapGet("", Handle)
            .WithName("GetDevices")
            .WithSummary("Get all devices")
            .WithDescription("Retrieves all devices for the specified tenant with pagination support")
            .RequirePermission("Identity.Device.Read", "View devices");
    }

    private static async Task<IResult> Handle(
        HttpContext context,
        string tenantId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var pagingParams = context.GetPagingParameters();
            var query = new GetDevicesQuery(tenantId, pagingParams);
            var result = await mediator.Send(query, cancellationToken);

            return Results.Ok(Response<PagedResponse<DeviceDto>>.SuccessResult(result, "Devices retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving devices"
            );
        }
    }
}
