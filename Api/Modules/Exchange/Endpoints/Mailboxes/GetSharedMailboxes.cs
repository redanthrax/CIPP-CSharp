using CIPP.Api.Extensions;
using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Queries.Mailboxes;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.Mailboxes;

public static class GetSharedMailboxes {
    public static void MapGetSharedMailboxes(this RouteGroupBuilder group) {
        group.MapGet("/{tenantId}/shared-mailboxes", Handle)
            .WithName("GetSharedMailboxes")
            .WithSummary("Get shared mailboxes")
            .WithDescription("Returns a paginated list of shared mailboxes for a tenant")
            .RequirePermission("CIPP.Exchange.Mailbox.Read", "View shared mailboxes");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        HttpContext context,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var pagingParams = context.GetPagingParameters();
            var query = new GetSharedMailboxesQuery(tenantId, pagingParams);
            var result = await mediator.Send(query, cancellationToken);

            return Results.Ok(Response<PagedResponse<SharedMailboxDto>>.SuccessResult(result, "Shared mailboxes retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving shared mailboxes"
            );
        }
    }
}
