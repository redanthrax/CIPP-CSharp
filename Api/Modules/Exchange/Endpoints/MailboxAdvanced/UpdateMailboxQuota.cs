using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Commands;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.MailboxAdvanced;

public static class UpdateMailboxQuota {
    public static void MapUpdateMailboxQuota(this RouteGroupBuilder group) {
        group.MapPut("/quota", Handle)
            .WithName("UpdateMailboxQuota")
            .WithSummary("Update mailbox quota")
            .WithDescription("Updates quota settings for a mailbox")
            .RequirePermission("Exchange.Mailbox.Write", "Manage mailboxes");
    }

    private static async Task<IResult> Handle(
        string tenantId,
        string mailboxId,
        UpdateMailboxQuotaDto updateDto,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new UpdateMailboxQuotaCommand(tenantId, mailboxId, updateDto);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<object>.SuccessResult(null, "Mailbox quota updated successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error updating mailbox quota"
            );
        }
    }
}
