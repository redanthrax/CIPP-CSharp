using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Commands;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.Mailboxes;

public static class UpdateMailboxForwarding {
    public static void MapUpdateMailboxForwarding(this RouteGroupBuilder group) {
        group.MapPost("/{userId}/forwarding", Handle)
            .WithName("UpdateMailboxForwarding")
            .WithSummary("Update mailbox forwarding")
            .WithDescription("Updates forwarding settings for a mailbox")
            .RequirePermission("Exchange.Mailbox.ReadWrite", "Manage mailbox forwarding");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string userId,
        UpdateMailboxForwardingDto updateDto,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new UpdateMailboxForwardingCommand(tenantId, userId, updateDto);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<string>.SuccessResult("Mailbox forwarding updated successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error updating mailbox forwarding"
            );
        }
    }
}
