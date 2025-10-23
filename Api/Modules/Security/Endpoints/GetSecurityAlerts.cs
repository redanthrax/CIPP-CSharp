using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Security.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Security;
using DispatchR;

namespace CIPP.Api.Modules.Security.Endpoints;

public static class GetSecurityAlerts {
    public static void MapGetSecurityAlerts(this RouteGroupBuilder group) {
        group.MapGet("/alerts", Handle)
            .WithName("GetSecurityAlerts")
            .WithSummary("Get all security alerts")
            .WithDescription("Retrieves all security alerts for the specified tenant with aggregated counts")
            .RequirePermission("Security.Alert.Read", "View security alerts");
    }

    private static async Task<IResult> Handle(
        string tenantId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetSecurityAlertsQuery(tenantId);
            var result = await mediator.Send(query, cancellationToken);

            return Results.Ok(Response<SecurityAlertsResponseDto>.SuccessResult(result, "Alerts retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving security alerts"
            );
        }
    }
}
