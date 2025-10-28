using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Commands.MailResources;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.MailResources;

public static class CreateEquipmentMailbox {
    public static void MapCreateEquipmentMailbox(this RouteGroupBuilder group) {
        group.MapPost("/{tenantId}/mail-resources/equipment", Handle)
            .WithName("CreateEquipmentMailbox")
            .WithSummary("Create equipment mailbox")
            .WithDescription("Creates a new equipment mailbox for a tenant")
            .RequirePermission("CIPP.Exchange.MailResources.ReadWrite", "Create equipment mailbox");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        CreateEquipmentMailboxDto createDto,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new CreateEquipmentMailboxCommand(tenantId, createDto);
            var result = await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<string>.SuccessResult(result, "Equipment mailbox created successfully"));
        } catch (InvalidOperationException ex) {
            return Results.BadRequest(Response<string>.ErrorResult(ex.Message));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error creating equipment mailbox"
            );
        }
    }
}
