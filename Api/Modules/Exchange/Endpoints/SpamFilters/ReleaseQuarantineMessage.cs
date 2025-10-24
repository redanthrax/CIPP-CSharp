using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Commands;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.SpamFilters;

public static class ReleaseQuarantineMessage {
    public static void MapReleaseQuarantineMessage(this RouteGroupBuilder group) {
        group.MapPost("/quarantine/{messageId}/release", Handle)
            .WithName("ReleaseQuarantineMessage")
            .WithSummary("Release quarantine message")
            .WithDescription("Releases a quarantined message to all original recipients")
            .RequirePermission("Exchange.SpamFilter.Write", "Manage spam filter policies");
    }

    private static async Task<IResult> Handle(
        string tenantId,
        string messageId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new ReleaseQuarantineMessageCommand(tenantId, messageId);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<object>.SuccessResult(null, "Quarantine message released successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error releasing quarantine message"
            );
        }
    }
}
