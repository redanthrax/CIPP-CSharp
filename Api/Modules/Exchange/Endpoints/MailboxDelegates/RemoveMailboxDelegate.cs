using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Commands.MailboxDelegates;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;
using Microsoft.AspNetCore.Mvc;

namespace CIPP.Api.Modules.Exchange.Endpoints.MailboxDelegates;

public static class RemoveMailboxDelegate {
    public static void MapRemoveMailboxDelegate(this RouteGroupBuilder group) {
        group.MapDelete("/delegates", Handle)
            .WithName("RemoveMailboxDelegate")
            .WithSummary("Remove mailbox delegate")
            .WithDescription("Removes delegate permissions from a mailbox")
            .RequirePermission("Exchange.Mailbox.ReadWrite", "Manage mailbox delegates");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string mailboxId,
        [FromBody] RemoveMailboxDelegateDto request,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            request.TenantId = tenantId;
            
            var command = new RemoveMailboxDelegateCommand(tenantId, mailboxId, request);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<object?>.SuccessResult(null, "Mailbox delegate removed successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error removing mailbox delegate"
            );
        }
    }
}
