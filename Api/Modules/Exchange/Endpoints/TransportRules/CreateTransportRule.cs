using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Commands;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.TransportRules;

public static class CreateTransportRule {
    public static void MapCreateTransportRule(this RouteGroupBuilder group) {
        group.MapPost("/", Handle)
            .WithName("CreateTransportRule")
            .WithSummary("Create transport rule")
            .WithDescription("Creates a new transport rule")
            .RequirePermission("Exchange.TransportRule.ReadWrite", "Create transport rules");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        CreateTransportRuleDto createDto,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new CreateTransportRuleCommand(tenantId, createDto);
            var ruleId = await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<string>.SuccessResult(ruleId, "Transport rule created successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error creating transport rule"
            );
        }
    }
}
