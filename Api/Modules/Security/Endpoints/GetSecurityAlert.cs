using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Security.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Security;
using DispatchR;

namespace CIPP.Api.Modules.Security.Endpoints;

public static class GetSecurityAlert {
    public static void MapGetSecurityAlert(this RouteGroupBuilder group) {
        group.MapGet("/alerts/{alertId}", Handle)
            .WithName("GetSecurityAlert")
            .WithSummary("Get a security alert")
            .WithDescription("Retrieves details of a specific security alert")
            .RequirePermission("Security.Alert.Read", "View security alerts");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string alertId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetSecurityAlertQuery(tenantId, alertId);
            var result = await mediator.Send(query, cancellationToken);

            if (result == null) {
                return Results.NotFound(Response<SecurityAlertDto>.ErrorResult("Alert not found"));
            }

            return Results.Ok(Response<SecurityAlertDto>.SuccessResult(result, "Alert retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving security alert"
            );
        }
    }
}
