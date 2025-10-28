using CIPP.Api.Modules.Exchange.Queries;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;
using Microsoft.AspNetCore.Mvc;

namespace CIPP.Api.Modules.Exchange.Endpoints;

public static class GetMailboxCalendarConfiguration {
    public static RouteGroupBuilder MapGetMailboxCalendarConfiguration(this RouteGroupBuilder group) {
        group.MapGet("/{tenantId:guid}/mailboxes/{mailboxId}/calendar-configuration", async (
            [FromRoute] Guid tenantId,
            [FromRoute] string mailboxId,
            [FromServices] IMediator mediator,
            CancellationToken cancellationToken
        ) => {
            var query = new GetMailboxCalendarConfigurationQuery(tenantId, mailboxId);
            var result = await mediator.Send(query, cancellationToken);
            
            if (result == null) {
                return Results.NotFound(new { message = "Calendar configuration not found" });
            }
            
            return Results.Ok(result);
        })
        .WithName("GetMailboxCalendarConfiguration")
        .WithTags("Exchange - Mailboxes")
        .WithDescription("Get calendar configuration for a mailbox")
        .Produces<MailboxCalendarConfigurationDto>(200)
        .Produces(404);

        return group;
    }
}
