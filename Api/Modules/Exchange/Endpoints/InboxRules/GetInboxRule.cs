using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.InboxRules;

public static class GetInboxRule {
    public static void MapGetInboxRule(this RouteGroupBuilder group) {
        group.MapGet("/{ruleId}", Handle)
            .WithName("GetInboxRule")
            .WithSummary("Get inbox rule")
            .WithDescription("Retrieves a specific inbox rule for a mailbox")
            .RequirePermission("Exchange.Mailbox.Read", "View mailbox rules");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string mailboxId,
        string ruleId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetInboxRuleQuery(tenantId, mailboxId, ruleId);
            var result = await mediator.Send(query, cancellationToken);

            if (result == null) {
                return Results.NotFound(Response<object>.ErrorResult("Inbox rule not found"));
            }

            return Results.Ok(Response<InboxRuleDto>.SuccessResult(result, "Inbox rule retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving inbox rule"
            );
        }
    }
}
