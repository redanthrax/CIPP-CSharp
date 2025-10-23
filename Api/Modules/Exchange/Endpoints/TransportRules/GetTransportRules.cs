using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.TransportRules;

public static class GetTransportRules {
    public static void MapGetTransportRules(this RouteGroupBuilder group) {
        group.MapGet("/", Handle)
            .WithName("GetTransportRules")
            .WithSummary("Get transport rules")
            .WithDescription("Retrieves all transport rules for a tenant")
            .RequirePermission("Exchange.TransportRule.Read", "View transport rules");
    }

    private static async Task<IResult> Handle(
        string tenantId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetTransportRulesQuery(tenantId);
            var result = await mediator.Send(query, cancellationToken);

            return Results.Ok(Response<List<TransportRuleDto>>.SuccessResult(result, "Transport rules retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving transport rules"
            );
        }
    }
}
