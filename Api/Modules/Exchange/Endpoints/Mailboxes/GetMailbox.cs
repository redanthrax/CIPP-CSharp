using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.Mailboxes;

public static class GetMailbox {
    public static void MapGetMailbox(this RouteGroupBuilder group) {
        group.MapGet("/{userId}", Handle)
            .WithName("GetMailbox")
            .WithSummary("Get mailbox")
            .WithDescription("Retrieves details for a specific mailbox")
            .RequirePermission("Exchange.Mailbox.Read", "View mailbox details");
    }

    private static async Task<IResult> Handle(
        string tenantId,
        string userId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetMailboxQuery(tenantId, userId);
            var result = await mediator.Send(query, cancellationToken);

            if (result == null) {
                return Results.NotFound(Response<MailboxDetailsDto>.ErrorResult("Mailbox not found"));
            }

            return Results.Ok(Response<MailboxDetailsDto>.SuccessResult(result, "Mailbox retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving mailbox"
            );
        }
    }
}
