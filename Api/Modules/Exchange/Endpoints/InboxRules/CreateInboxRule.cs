using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Commands;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.InboxRules;

public static class CreateInboxRule {
    public static void MapCreateInboxRule(this RouteGroupBuilder group) {
        group.MapPost("/", Handle)
            .WithName("CreateInboxRule")
            .WithSummary("Create inbox rule")
            .WithDescription("Creates a new inbox rule for a mailbox")
            .RequirePermission("Exchange.Mailbox.Write", "Manage mailbox rules");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string mailboxId,
        CreateInboxRuleDto createDto,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new CreateInboxRuleCommand(tenantId, mailboxId, createDto);
            var ruleName = await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<string>.SuccessResult(ruleName, "Inbox rule created successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error creating inbox rule"
            );
        }
    }
}
