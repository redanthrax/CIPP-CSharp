using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Commands;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.InboxRules;

public static class DeleteInboxRule {
    public static void MapDeleteInboxRule(this RouteGroupBuilder group) {
        group.MapDelete("/{ruleId}", Handle)
            .WithName("DeleteInboxRule")
            .WithSummary("Delete inbox rule")
            .WithDescription("Deletes an inbox rule from a mailbox")
            .RequirePermission("Exchange.Mailbox.Write", "Manage mailbox rules");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string mailboxId,
        string ruleId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new DeleteInboxRuleCommand(tenantId, mailboxId, ruleId);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<object>.SuccessResult(null, "Inbox rule deleted successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error deleting inbox rule"
            );
        }
    }
}
