using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Commands.ActiveSync;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.ActiveSync;

public static class CreateActiveSyncDeviceAccessRule {
    public static void MapCreateActiveSyncDeviceAccessRule(this RouteGroupBuilder group) {
        group.MapPost("/{tenantId}/activesync/device-access-rules", Handle)
            .WithName("CreateActiveSyncDeviceAccessRule")
            .WithSummary("Create ActiveSync device access rule")
            .WithDescription("Creates a new ActiveSync device access rule for a tenant")
            .RequirePermission("CIPP.Exchange.ActiveSync.ReadWrite", "Create ActiveSync device access rule");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        CreateActiveSyncDeviceAccessRuleDto ruleData,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new CreateActiveSyncDeviceAccessRuleCommand(tenantId, ruleData);
            var result = await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<string>.SuccessResult(result, "ActiveSync device access rule created successfully"));
        } catch (InvalidOperationException ex) {
            return Results.BadRequest(Response<string>.ErrorResult(ex.Message));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error creating ActiveSync device access rule"
            );
        }
    }
}
