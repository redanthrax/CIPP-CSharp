using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Commands;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.InboxRules;

public static class UpdateInboxRule {
    public static void MapUpdateInboxRule(this RouteGroupBuilder group) {
        group.MapPut("/{ruleId}", Handle)
            .WithName("UpdateInboxRule")
            .WithSummary("Update inbox rule")
            .WithDescription("Updates an existing inbox rule for a mailbox")
            .RequirePermission("Exchange.Mailbox.Write", "Manage mailbox rules");
    }

    private static async Task<IResult> Handle(
        string tenantId,
        string mailboxId,
        string ruleId,
        UpdateInboxRuleDto updateDto,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new UpdateInboxRuleCommand(tenantId, mailboxId, ruleId, updateDto);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<object>.SuccessResult(null, "Inbox rule updated successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error updating inbox rule"
            );
        }
    }
}
