using CIPP.Api.Extensions;
using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Queries.MobileDevices;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.MobileDevices;

public static class GetMobileDevices {
    public static void MapGetMobileDevices(this RouteGroupBuilder group) {
        group.MapGet("/", Handle)
            .WithName("GetMobileDevices")
            .WithSummary("List mobile devices")
            .WithDescription("Retrieves all mobile devices for the specified tenant")
            .RequirePermission("Exchange.MobileDevice.Read", "View mobile devices");
    }

    private static async Task<IResult> Handle(
        HttpContext context,
        Guid tenantId,
        string? mailbox,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var pagingParams = context.GetPagingParameters();
            var query = new GetMobileDevicesQuery(tenantId, mailbox, pagingParams);
            var result = await mediator.Send(query, cancellationToken);

            return Results.Ok(Response<PagedResponse<MobileDeviceDto>>.SuccessResult(result, "Mobile devices retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(detail: ex.Message, statusCode: 500, title: "Error retrieving mobile devices");
        }
    }
}