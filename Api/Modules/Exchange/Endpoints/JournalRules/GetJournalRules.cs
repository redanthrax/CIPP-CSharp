using CIPP.Api.Extensions;
using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Queries.JournalRules;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.JournalRules;

public static class GetJournalRules {
    public static void MapGetJournalRules(this RouteGroupBuilder group) {
        group.MapGet("/journal-rules", Handle)
            .WithName("GetJournalRules")
            .WithSummary("Get journal rules")
            .WithDescription("Retrieves all journal rules for the specified tenant")
            .RequirePermission("Exchange.JournalRules.Read", "View journal rules");
    }

    private static async Task<IResult> Handle(
        HttpContext context,
        Guid tenantId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var pagingParams = context.GetPagingParameters();
            var query = new GetJournalRulesQuery(tenantId, pagingParams);
            var rules = await mediator.Send(query, cancellationToken);

            return Results.Ok(Response<PagedResponse<JournalRuleDto>>.SuccessResult(rules, "Journal rules retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving journal rules"
            );
        }
    }
}
