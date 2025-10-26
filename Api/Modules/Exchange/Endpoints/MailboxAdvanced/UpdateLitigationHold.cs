using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Commands;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.MailboxAdvanced;

public static class UpdateLitigationHold {
    public static void MapUpdateLitigationHold(this RouteGroupBuilder group) {
        group.MapPut("/litigation-hold", Handle)
            .WithName("UpdateLitigationHold")
            .WithSummary("Update litigation hold")
            .WithDescription("Updates litigation hold settings for a mailbox")
            .RequirePermission("Exchange.Mailbox.Write", "Manage mailboxes");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string mailboxId,
        LitigationHoldDto holdDto,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new UpdateLitigationHoldCommand(tenantId, mailboxId, holdDto);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<object>.SuccessResult(null, "Litigation hold updated successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error updating litigation hold"
            );
        }
    }
}
