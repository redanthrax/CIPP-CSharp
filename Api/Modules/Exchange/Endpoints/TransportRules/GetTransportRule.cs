using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.TransportRules;

public static class GetTransportRule {
    public static void MapGetTransportRule(this RouteGroupBuilder group) {
        group.MapGet("/{ruleId}", Handle)
            .WithName("GetTransportRule")
            .WithSummary("Get transport rule")
            .WithDescription("Retrieves details for a specific transport rule")
            .RequirePermission("Exchange.TransportRule.Read", "View transport rule details");
    }

    private static async Task<IResult> Handle(
        string tenantId,
        string ruleId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetTransportRuleQuery(tenantId, ruleId);
            var result = await mediator.Send(query, cancellationToken);

            if (result == null) {
                return Results.NotFound(Response<TransportRuleDetailsDto>.ErrorResult("Transport rule not found"));
            }

            return Results.Ok(Response<TransportRuleDetailsDto>.SuccessResult(result, "Transport rule retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving transport rule"
            );
        }
    }
}
