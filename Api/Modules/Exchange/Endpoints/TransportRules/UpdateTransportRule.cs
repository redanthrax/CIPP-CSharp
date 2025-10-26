using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Commands;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.TransportRules;

public static class UpdateTransportRule {
    public static void MapUpdateTransportRule(this RouteGroupBuilder group) {
        group.MapPatch("/{ruleId}", Handle)
            .WithName("UpdateTransportRule")
            .WithSummary("Update transport rule")
            .WithDescription("Updates a transport rule")
            .RequirePermission("Exchange.TransportRule.ReadWrite", "Update transport rules");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string ruleId,
        UpdateTransportRuleDto updateDto,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new UpdateTransportRuleCommand(tenantId, ruleId, updateDto);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<string>.SuccessResult("Transport rule updated successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error updating transport rule"
            );
        }
    }
}
