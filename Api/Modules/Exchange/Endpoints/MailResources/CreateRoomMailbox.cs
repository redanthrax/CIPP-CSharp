using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Commands.MailResources;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.MailResources;

public static class CreateRoomMailbox {
    public static void MapCreateRoomMailbox(this RouteGroupBuilder group) {
        group.MapPost("/{tenantId}/mail-resources/rooms", Handle)
            .WithName("CreateRoomMailbox")
            .WithSummary("Create room mailbox")
            .WithDescription("Creates a new room mailbox for a tenant")
            .RequirePermission("CIPP.Exchange.MailResources.ReadWrite", "Create room mailbox");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        CreateRoomMailboxDto createDto,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new CreateRoomMailboxCommand(tenantId, createDto);
            var result = await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<string>.SuccessResult(result, "Room mailbox created successfully"));
        } catch (InvalidOperationException ex) {
            return Results.BadRequest(Response<string>.ErrorResult(ex.Message));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error creating room mailbox"
            );
        }
    }
}
