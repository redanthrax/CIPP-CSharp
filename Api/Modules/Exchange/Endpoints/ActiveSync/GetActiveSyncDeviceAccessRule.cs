using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Queries.ActiveSync;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.ActiveSync;

public static class GetActiveSyncDeviceAccessRule {
    public static void MapGetActiveSyncDeviceAccessRule(this RouteGroupBuilder group) {
        group.MapGet("/{tenantId}/activesync/device-access-rules/{ruleId}", Handle)
            .WithName("GetActiveSyncDeviceAccessRule")
            .WithSummary("Get ActiveSync device access rule")
            .WithDescription("Returns a specific ActiveSync device access rule by ID")
            .RequirePermission("CIPP.Exchange.ActiveSync.Read", "View ActiveSync device access rule");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string ruleId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetActiveSyncDeviceAccessRuleQuery(tenantId, ruleId);
            var result = await mediator.Send(query, cancellationToken);

            if (result == null) {
                return Results.NotFound(Response<ActiveSyncDeviceAccessRuleDto>.ErrorResult("ActiveSync device access rule not found"));
            }

            return Results.Ok(Response<ActiveSyncDeviceAccessRuleDto>.SuccessResult(result, "ActiveSync device access rule retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving ActiveSync device access rule"
            );
        }
    }
}
