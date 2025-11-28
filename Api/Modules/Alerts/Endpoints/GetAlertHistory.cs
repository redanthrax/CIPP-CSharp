using CIPP.Api.Extensions;
using CIPP.Api.Modules.Alerts.Queries;
using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Alerts;
using DispatchR;

namespace CIPP.Api.Modules.Alerts.Endpoints;

public static class GetAlertHistory {
    public static void MapGetAlertHistory(this RouteGroupBuilder group) {
        group.MapGet("/history", Handle)
            .WithName("GetAlertHistory")
            .WithSummary("Get alert history")
            .WithDescription("Returns a paginated list of triggered alerts and audit logs")
            .RequirePermission("CIPP.Alert.Read", "View alert history");
    }

    private static async Task<IResult> Handle(
        HttpContext context,
        IMediator mediator,
        string? tenantFilter,
        DateTime? startDate,
        DateTime? endDate,
        string? logType,
        CancellationToken cancellationToken = default) {
        try {
            var pagingParams = context.GetPagingParameters();
            var query = new GetAlertHistoryQuery(
                tenantFilter,
                startDate,
                endDate,
                logType,
                pagingParams);

            var result = await mediator.Send(query, cancellationToken);
            return Results.Ok(Response<PagedResponse<AlertHistoryDto>>.SuccessResult(result, "Alert history retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving alert history"
            );
        }
    }
}
