using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Commands;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.SpamFilters;

public static class DeleteQuarantineMessage {
    public static void MapDeleteQuarantineMessage(this RouteGroupBuilder group) {
        group.MapDelete("/quarantine/{messageId}", Handle)
            .WithName("DeleteQuarantineMessage")
            .WithSummary("Delete quarantine message")
            .WithDescription("Deletes a quarantined message")
            .RequirePermission("Exchange.SpamFilter.Write", "Manage spam filter policies");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string messageId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new DeleteQuarantineMessageCommand(tenantId, messageId);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<object>.SuccessResult(new { }, "Quarantine message deleted successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error deleting quarantine message"
            );
        }
    }
}
