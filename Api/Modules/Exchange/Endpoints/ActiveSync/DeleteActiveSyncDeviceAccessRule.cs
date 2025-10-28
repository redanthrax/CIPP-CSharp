using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Commands.ActiveSync;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.ActiveSync;

public static class DeleteActiveSyncDeviceAccessRule {
    public static void MapDeleteActiveSyncDeviceAccessRule(this RouteGroupBuilder group) {
        group.MapDelete("/{tenantId}/activesync/device-access-rules/{ruleId}", Handle)
            .WithName("DeleteActiveSyncDeviceAccessRule")
            .WithSummary("Delete ActiveSync device access rule")
            .WithDescription("Deletes an ActiveSync device access rule")
            .RequirePermission("CIPP.Exchange.ActiveSync.ReadWrite", "Delete ActiveSync device access rule");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string ruleId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new DeleteActiveSyncDeviceAccessRuleCommand(tenantId, ruleId);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<string>.SuccessResult("ActiveSync device access rule deleted successfully"));
        } catch (InvalidOperationException ex) {
            return Results.NotFound(Response<string>.ErrorResult(ex.Message));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error deleting ActiveSync device access rule"
            );
        }
    }
}
