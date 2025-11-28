using CIPP.Api.Extensions;
using CIPP.Api.Modules.Alerts.Queries;
using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Alerts;
using DispatchR;

namespace CIPP.Api.Modules.Alerts.Endpoints;

public static class GetAlertStatistics {
    public static void MapGetAlertStatistics(this RouteGroupBuilder group) {
        group.MapGet("/statistics", Handle)
            .WithName("GetAlertStatistics")
            .WithSummary("Get alert statistics")
            .WithDescription("Returns statistics about triggered alerts including counts and breakdowns")
            .RequirePermission("CIPP.Alert.Read", "View alert statistics");
    }

    private static async Task<IResult> Handle(
        HttpContext context,
        IMediator mediator,
        DateTime? startDate,
        DateTime? endDate,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetAlertStatisticsQuery(startDate, endDate);
            var result = await mediator.Send(query, cancellationToken);
            return Results.Ok(Response<AlertStatisticsDto>.SuccessResult(result, "Alert statistics retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving alert statistics"
            );
        }
    }
}
