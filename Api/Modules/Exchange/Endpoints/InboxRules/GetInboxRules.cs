using CIPP.Api.Extensions;
using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.InboxRules;

public static class GetInboxRules {
    public static void MapGetInboxRules(this RouteGroupBuilder group) {
        group.MapGet("/", Handle)
            .WithName("GetInboxRules")
            .WithSummary("Get inbox rules")
            .WithDescription("Retrieves all inbox rules for a mailbox with pagination support")
            .RequirePermission("Exchange.Mailbox.Read", "View mailbox rules");
    }

    private static async Task<IResult> Handle(
        HttpContext context,
        string tenantId,
        string mailboxId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var pagingParams = context.GetPagingParameters();
            var query = new GetInboxRulesQuery(tenantId, mailboxId, pagingParams);
            var result = await mediator.Send(query, cancellationToken);

            return Results.Ok(Response<PagedResponse<InboxRuleDto>>.SuccessResult(result, "Inbox rules retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving inbox rules"
            );
        }
    }
}
