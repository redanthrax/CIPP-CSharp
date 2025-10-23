using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.Mailboxes;

public static class GetMailboxForwarding {
    public static void MapGetMailboxForwarding(this RouteGroupBuilder group) {
        group.MapGet("/{userId}/forwarding", Handle)
            .WithName("GetMailboxForwarding")
            .WithSummary("Get mailbox forwarding")
            .WithDescription("Retrieves forwarding settings for a mailbox")
            .RequirePermission("Exchange.Mailbox.Read", "View mailbox forwarding");
    }

    private static async Task<IResult> Handle(
        string tenantId,
        string userId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetMailboxForwardingQuery(tenantId, userId);
            var result = await mediator.Send(query, cancellationToken);

            return Results.Ok(Response<MailboxForwardingDto>.SuccessResult(result, "Mailbox forwarding retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving mailbox forwarding"
            );
        }
    }
}
