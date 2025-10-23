using CIPP.Api.Extensions;
using CIPP.Api.Modules.Alerts.Queries;
using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Alerts;
using DispatchR;

namespace CIPP.Api.Modules.Alerts.Endpoints;

public static class GetAlertConfigurations {
    public static void MapGetAlertConfigurations(this RouteGroupBuilder group) {
        group.MapGet("/", Handle)
            .WithName("GetAlertConfigurations")
            .WithSummary("Get alert configurations")
            .WithDescription("Returns a paginated list of configured alerts including audit log alerts and scheduled tasks")
            .RequirePermission("CIPP.Alert.Read", "View alert configurations");
    }

    private static async Task<IResult> Handle(
        HttpContext context,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var pagingParams = context.GetPagingParameters();
            var query = new GetAlertConfigurationsQuery(pagingParams);
            var alerts = await mediator.Send(query, cancellationToken);

            return Results.Ok(Response<PagedResponse<AlertConfigurationDto>>.SuccessResult(alerts, "Alert configurations retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving alert configurations"
            );
        }
    }
}
