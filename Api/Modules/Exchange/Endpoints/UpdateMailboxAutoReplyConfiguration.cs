using CIPP.Api.Modules.Exchange.Commands;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;
using Microsoft.AspNetCore.Mvc;

namespace CIPP.Api.Modules.Exchange.Endpoints;

public static class UpdateMailboxAutoReplyConfiguration {
    public static RouteGroupBuilder MapUpdateMailboxAutoReplyConfiguration(this RouteGroupBuilder group) {
        group.MapPut("/{tenantId:guid}/mailboxes/{mailboxId}/auto-reply", async (
            [FromRoute] Guid tenantId,
            [FromRoute] string mailboxId,
            [FromBody] UpdateMailboxAutoReplyConfigurationDto updateDto,
            [FromServices] IMediator mediator,
            CancellationToken cancellationToken
        ) => {
            var command = new UpdateMailboxAutoReplyConfigurationCommand(tenantId, mailboxId, updateDto);
            await mediator.Send(command, cancellationToken);
            
            return Results.Ok(new { message = "Auto-reply configuration updated successfully" });
        })
        .WithName("UpdateMailboxAutoReplyConfiguration")
        .WithTags("Exchange - Mailboxes")
        .WithDescription("Update auto-reply (out of office) configuration for a mailbox")
        .Produces(200)
        .Produces(400);

        return group;
    }
}
