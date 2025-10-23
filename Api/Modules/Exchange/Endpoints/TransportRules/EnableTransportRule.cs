using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Commands;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.TransportRules;

public static class EnableTransportRule {
    public static void MapEnableTransportRule(this RouteGroupBuilder group) {
        group.MapPost("/{ruleId}/enable", Handle)
            .WithName("EnableTransportRule")
            .WithSummary("Enable/Disable transport rule")
            .WithDescription("Enables or disables a transport rule")
            .RequirePermission("Exchange.TransportRule.ReadWrite", "Enable/disable transport rules");
    }

    private static async Task<IResult> Handle(
        string tenantId,
        string ruleId,
        bool enable,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new EnableTransportRuleCommand(tenantId, ruleId, enable);
            await mediator.Send(command, cancellationToken);

            var message = enable ? "Transport rule enabled successfully" : "Transport rule disabled successfully";
            return Results.Ok(Response<string>.SuccessResult(message));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error enabling/disabling transport rule"
            );
        }
    }
}
