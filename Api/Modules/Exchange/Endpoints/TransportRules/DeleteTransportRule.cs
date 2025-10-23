using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Commands;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.TransportRules;

public static class DeleteTransportRule {
    public static void MapDeleteTransportRule(this RouteGroupBuilder group) {
        group.MapDelete("/{ruleId}", Handle)
            .WithName("DeleteTransportRule")
            .WithSummary("Delete transport rule")
            .WithDescription("Deletes a transport rule")
            .RequirePermission("Exchange.TransportRule.ReadWrite", "Delete transport rules");
    }

    private static async Task<IResult> Handle(
        string tenantId,
        string ruleId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new DeleteTransportRuleCommand(tenantId, ruleId);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<string>.SuccessResult("Transport rule deleted successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error deleting transport rule"
            );
        }
    }
}
