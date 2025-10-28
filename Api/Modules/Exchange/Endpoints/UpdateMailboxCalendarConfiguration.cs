using CIPP.Api.Modules.Exchange.Commands;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;
using Microsoft.AspNetCore.Mvc;

namespace CIPP.Api.Modules.Exchange.Endpoints;

public static class UpdateMailboxCalendarConfiguration {
    public static RouteGroupBuilder MapUpdateMailboxCalendarConfiguration(this RouteGroupBuilder group) {
        group.MapPut("/{tenantId:guid}/mailboxes/{mailboxId}/calendar-configuration", async (
            [FromRoute] Guid tenantId,
            [FromRoute] string mailboxId,
            [FromBody] UpdateMailboxCalendarConfigurationDto updateDto,
            [FromServices] IMediator mediator,
            CancellationToken cancellationToken
        ) => {
            var command = new UpdateMailboxCalendarConfigurationCommand(tenantId, mailboxId, updateDto);
            await mediator.Send(command, cancellationToken);
            
            return Results.Ok(new { message = "Calendar configuration updated successfully" });
        })
        .WithName("UpdateMailboxCalendarConfiguration")
        .WithTags("Exchange - Mailboxes")
        .WithDescription("Update calendar configuration for a mailbox")
        .Produces(200)
        .Produces(400);

        return group;
    }
}
