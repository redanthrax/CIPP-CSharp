using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Security.Commands;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Security;
using DispatchR;

namespace CIPP.Api.Modules.Security.Endpoints;

public static class UpdateSecurityAlert {
    public static void MapUpdateSecurityAlert(this RouteGroupBuilder group) {
        group.MapPatch("/alerts/{alertId}", Handle)
            .WithName("UpdateSecurityAlert")
            .WithSummary("Update a security alert")
            .WithDescription("Updates properties of a specific security alert")
            .RequirePermission("Security.Alert.Write", "Update security alerts");
    }

    private static async Task<IResult> Handle(
        string tenantId,
        string alertId,
        UpdateSecurityAlertDto updateDto,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            updateDto.TenantId = tenantId;
            var command = new UpdateSecurityAlertCommand(tenantId, alertId, updateDto);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<string>.SuccessResult("Alert updated successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error updating security alert"
            );
        }
    }
}
