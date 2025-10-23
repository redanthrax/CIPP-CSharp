using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Commands;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.Mailboxes;

public static class UpdateMailboxPermissions {
    public static void MapUpdateMailboxPermissions(this RouteGroupBuilder group) {
        group.MapPost("/{userId}/permissions", Handle)
            .WithName("UpdateMailboxPermissions")
            .WithSummary("Update mailbox permissions")
            .WithDescription("Updates permissions for a mailbox (add or remove)")
            .RequirePermission("Exchange.Mailbox.ReadWrite", "Manage mailbox permissions");
    }

    private static async Task<IResult> Handle(
        string tenantId,
        string userId,
        UpdateMailboxPermissionsDto updateDto,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new UpdateMailboxPermissionsCommand(tenantId, userId, updateDto);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<string>.SuccessResult("Mailbox permissions updated successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error updating mailbox permissions"
            );
        }
    }
}
