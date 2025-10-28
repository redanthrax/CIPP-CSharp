using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Queries.MailboxDelegates;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.MailboxDelegates;

public static class GetMailboxDelegates {
    public static void MapGetMailboxDelegates(this RouteGroupBuilder group) {
        group.MapGet("/delegates", Handle)
            .WithName("GetMailboxDelegates")
            .WithSummary("Get mailbox delegates")
            .WithDescription("Retrieves all delegate permissions for a mailbox")
            .RequirePermission("Exchange.Mailbox.Read", "View mailbox delegates");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string mailboxId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetMailboxDelegatesQuery(tenantId, mailboxId);
            var delegates = await mediator.Send(query, cancellationToken);

            return Results.Ok(Response<List<MailboxDelegateDto>>.SuccessResult(delegates, "Mailbox delegates retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving mailbox delegates"
            );
        }
    }
}
