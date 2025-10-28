using CIPP.Api.Extensions;
using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Queries.ActiveSync;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.ActiveSync;

public static class GetActiveSyncDeviceAccessRules {
    public static void MapGetActiveSyncDeviceAccessRules(this RouteGroupBuilder group) {
        group.MapGet("/{tenantId}/activesync/device-access-rules", Handle)
            .WithName("GetActiveSyncDeviceAccessRules")
            .WithSummary("Get ActiveSync device access rules")
            .WithDescription("Returns a paginated list of ActiveSync device access rules for a tenant")
            .RequirePermission("CIPP.Exchange.ActiveSync.Read", "View ActiveSync device access rules");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        HttpContext context,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var pagingParams = context.GetPagingParameters();
            var query = new GetActiveSyncDeviceAccessRulesQuery(tenantId, pagingParams);
            var result = await mediator.Send(query, cancellationToken);

            return Results.Ok(Response<PagedResponse<ActiveSyncDeviceAccessRuleDto>>.SuccessResult(result, "ActiveSync device access rules retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving ActiveSync device access rules"
            );
        }
    }
}
