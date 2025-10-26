using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Commands;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.Mailboxes;

public static class UpdateMailbox {
    public static void MapUpdateMailbox(this RouteGroupBuilder group) {
        group.MapPatch("/{userId}", Handle)
            .WithName("UpdateMailbox")
            .WithSummary("Update mailbox")
            .WithDescription("Updates mailbox settings")
            .RequirePermission("Exchange.Mailbox.ReadWrite", "Manage mailbox settings");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string userId,
        UpdateMailboxDto updateDto,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new UpdateMailboxCommand(tenantId, userId, updateDto);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<string>.SuccessResult("Mailbox updated successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error updating mailbox"
            );
        }
    }
}
