using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Commands.MailboxDelegates;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;
using Microsoft.AspNetCore.Mvc;

namespace CIPP.Api.Modules.Exchange.Endpoints.MailboxDelegates;

public static class AddMailboxDelegate {
    public static void MapAddMailboxDelegate(this RouteGroupBuilder group) {
        group.MapPost("/delegates", Handle)
            .WithName("AddMailboxDelegate")
            .WithSummary("Add mailbox delegate")
            .WithDescription("Adds delegate permissions to a mailbox (Full Access, Send As, Send on Behalf)")
            .RequirePermission("Exchange.Mailbox.ReadWrite", "Manage mailbox delegates");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string mailboxId,
        [FromBody] AddMailboxDelegateDto request,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            request.TenantId = tenantId;
            
            var command = new AddMailboxDelegateCommand(tenantId, mailboxId, request);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<object?>.SuccessResult(null, "Mailbox delegate added successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error adding mailbox delegate"
            );
        }
    }
}
