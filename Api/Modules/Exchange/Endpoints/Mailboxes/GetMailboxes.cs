using CIPP.Api.Extensions;
using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.Mailboxes;

public static class GetMailboxes {
    public static void MapGetMailboxes(this RouteGroupBuilder group) {
        group.MapGet("/", Handle)
            .WithName("GetMailboxes")
            .WithSummary("Get mailboxes")
            .WithDescription("Retrieves all mailboxes for a tenant, optionally filtered by mailbox type")
            .RequirePermission("Exchange.Mailbox.Read", "View mailboxes");
    }

    private static async Task<IResult> Handle(
        HttpContext context,
        Guid tenantId,
        string? mailboxType,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var pagingParams = context.GetPagingParameters();
            var query = new GetMailboxesQuery(tenantId, mailboxType, pagingParams);
            var result = await mediator.Send(query, cancellationToken);

            return Results.Ok(Response<PagedResponse<MailboxDto>>.SuccessResult(result, "Mailboxes retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving mailboxes"
            );
        }
    }
}
