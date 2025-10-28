using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Commands.ActiveSync;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.ActiveSync;

public static class UpdateActiveSyncDeviceAccessRule {
    public static void MapUpdateActiveSyncDeviceAccessRule(this RouteGroupBuilder group) {
        group.MapPut("/{tenantId}/activesync/device-access-rules/{ruleId}", Handle)
            .WithName("UpdateActiveSyncDeviceAccessRule")
            .WithSummary("Update ActiveSync device access rule")
            .WithDescription("Updates an existing ActiveSync device access rule")
            .RequirePermission("CIPP.Exchange.ActiveSync.ReadWrite", "Update ActiveSync device access rule");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string ruleId,
        UpdateActiveSyncDeviceAccessRuleDto ruleData,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new UpdateActiveSyncDeviceAccessRuleCommand(tenantId, ruleId, ruleData);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<string>.SuccessResult("ActiveSync device access rule updated successfully"));
        } catch (InvalidOperationException ex) {
            return Results.BadRequest(Response<string>.ErrorResult(ex.Message));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error updating ActiveSync device access rule"
            );
        }
    }
}
