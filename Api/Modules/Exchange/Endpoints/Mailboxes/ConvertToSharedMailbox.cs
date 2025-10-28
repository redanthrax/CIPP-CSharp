using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Commands.Mailboxes;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.Mailboxes;

public static class ConvertToSharedMailbox {
    public static void MapConvertToSharedMailbox(this RouteGroupBuilder group) {
        group.MapPost("/{tenantId}/mailboxes/{identity}/convert-to-shared", Handle)
            .WithName("ConvertToSharedMailbox")
            .WithSummary("Convert mailbox to shared")
            .WithDescription("Converts an existing user mailbox to a shared mailbox")
            .RequirePermission("CIPP.Exchange.Mailbox.ReadWrite", "Convert mailbox to shared");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string identity,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new ConvertToSharedMailboxCommand(tenantId, identity);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<string>.SuccessResult("Mailbox converted to shared successfully"));
        } catch (InvalidOperationException ex) {
            return Results.BadRequest(Response<string>.ErrorResult(ex.Message));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error converting mailbox to shared"
            );
        }
    }
}
