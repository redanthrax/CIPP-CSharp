using CIPP.Api.Modules.Exchange.Queries;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;
using Microsoft.AspNetCore.Mvc;

namespace CIPP.Api.Modules.Exchange.Endpoints;

public static class GetMailboxAutoReplyConfiguration {
    public static RouteGroupBuilder MapGetMailboxAutoReplyConfiguration(this RouteGroupBuilder group) {
        group.MapGet("/{tenantId:guid}/mailboxes/{mailboxId}/auto-reply", async (
            [FromRoute] Guid tenantId,
            [FromRoute] string mailboxId,
            [FromServices] IMediator mediator,
            CancellationToken cancellationToken
        ) => {
            var query = new GetMailboxAutoReplyConfigurationQuery(tenantId, mailboxId);
            var result = await mediator.Send(query, cancellationToken);
            
            if (result == null) {
                return Results.NotFound(new { message = "Auto-reply configuration not found" });
            }
            
            return Results.Ok(result);
        })
        .WithName("GetMailboxAutoReplyConfiguration")
        .WithTags("Exchange - Mailboxes")
        .WithDescription("Get auto-reply (out of office) configuration for a mailbox")
        .Produces<MailboxAutoReplyConfigurationDto>(200)
        .Produces(404);

        return group;
    }
}
