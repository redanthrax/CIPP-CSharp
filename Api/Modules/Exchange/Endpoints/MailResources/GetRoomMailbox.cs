using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Queries.MailResources;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.MailResources;

public static class GetRoomMailbox {
    public static void MapGetRoomMailbox(this RouteGroupBuilder group) {
        group.MapGet("/{tenantId}/mail-resources/rooms/{identity}", Handle)
            .WithName("GetRoomMailbox")
            .WithSummary("Get room mailbox")
            .WithDescription("Returns a specific room mailbox by identity")
            .RequirePermission("CIPP.Exchange.MailResources.Read", "View room mailbox");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string identity,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetRoomMailboxQuery(tenantId, identity);
            var result = await mediator.Send(query, cancellationToken);

            if (result == null) {
                return Results.NotFound(Response<RoomMailboxDto>.ErrorResult("Room mailbox not found"));
            }

            return Results.Ok(Response<RoomMailboxDto>.SuccessResult(result, "Room mailbox retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving room mailbox"
            );
        }
    }
}
