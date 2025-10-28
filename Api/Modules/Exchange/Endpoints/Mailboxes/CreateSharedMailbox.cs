using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Commands.Mailboxes;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.Mailboxes;

public static class CreateSharedMailbox {
    public static void MapCreateSharedMailbox(this RouteGroupBuilder group) {
        group.MapPost("/{tenantId}/shared-mailboxes", Handle)
            .WithName("CreateSharedMailbox")
            .WithSummary("Create shared mailbox")
            .WithDescription("Creates a new shared mailbox for a tenant")
            .RequirePermission("CIPP.Exchange.Mailbox.ReadWrite", "Create shared mailbox");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        CreateSharedMailboxDto createDto,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new CreateSharedMailboxCommand(tenantId, createDto);
            var result = await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<string>.SuccessResult(result, "Shared mailbox created successfully"));
        } catch (InvalidOperationException ex) {
            return Results.BadRequest(Response<string>.ErrorResult(ex.Message));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error creating shared mailbox"
            );
        }
    }
}
