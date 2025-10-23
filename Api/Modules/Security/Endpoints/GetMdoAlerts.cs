using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Security.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Security;
using DispatchR;

namespace CIPP.Api.Modules.Security.Endpoints;

public static class GetMdoAlerts {
    public static void MapGetMdoAlerts(this RouteGroupBuilder group) {
        group.MapGet("/mdo-alerts", Handle)
            .WithName("GetMdoAlerts")
            .WithSummary("Get Microsoft Defender for Office 365 alerts")
            .WithDescription("Retrieves security alerts specifically from Microsoft Defender for Office 365")
            .RequirePermission("Security.Alert.Read", "View MDO security alerts");
    }

    private static async Task<IResult> Handle(
        string tenantId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetSecurityAlertsQuery(tenantId, "microsoftDefenderForOffice365");
            var result = await mediator.Send(query, cancellationToken);

            return Results.Ok(Response<SecurityAlertsResponseDto>.SuccessResult(result, "MDO alerts retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving MDO alerts"
            );
        }
    }
}
