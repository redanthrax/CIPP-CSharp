using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Commands;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.MailboxAdvanced;

public static class EnableArchive {
    public static void MapEnableArchive(this RouteGroupBuilder group) {
        group.MapPost("/enable-archive", Handle)
            .WithName("EnableMailboxArchive")
            .WithSummary("Enable mailbox archive")
            .WithDescription("Enables archive mailbox for a user")
            .RequirePermission("Exchange.Mailbox.Write", "Manage mailboxes");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string mailboxId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new EnableArchiveCommand(tenantId, mailboxId);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<object>.SuccessResult(null, "Mailbox archive enabled successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error enabling mailbox archive"
            );
        }
    }
}
