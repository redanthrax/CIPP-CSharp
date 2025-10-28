using CIPP.Api.Extensions;
using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Queries.MailResources;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.MailResources;

public static class GetRoomMailboxes {
    public static void MapGetRoomMailboxes(this RouteGroupBuilder group) {
        group.MapGet("/{tenantId}/mail-resources/rooms", Handle)
            .WithName("GetRoomMailboxes")
            .WithSummary("Get room mailboxes")
            .WithDescription("Returns a paginated list of room mailboxes for a tenant")
            .RequirePermission("CIPP.Exchange.MailResources.Read", "View room mailboxes");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        HttpContext context,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var pagingParams = context.GetPagingParameters();
            var query = new GetRoomMailboxesQuery(tenantId, pagingParams);
            var result = await mediator.Send(query, cancellationToken);

            return Results.Ok(Response<PagedResponse<RoomMailboxDto>>.SuccessResult(result, "Room mailboxes retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving room mailboxes"
            );
        }
    }
}
