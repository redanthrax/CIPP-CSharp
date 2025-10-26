using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Security.Commands;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Security;
using DispatchR;

namespace CIPP.Api.Modules.Security.Endpoints;

public static class UpdateSecureScoreControl {
    public static void MapUpdateSecureScoreControl(this RouteGroupBuilder group) {
        group.MapPatch("/control-profiles/{controlName}", Handle)
            .WithName("UpdateSecureScoreControl")
            .WithSummary("Update Secure Score control status")
            .WithDescription("Updates the state of a secure score control profile")
            .RequirePermission("Tenant.Administration.ReadWrite", "Update secure score control");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string controlName,
        UpdateSecureScoreControlDto updateDto,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new UpdateSecureScoreControlCommand(tenantId, controlName, updateDto);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<object?>.SuccessResult(null, $"Successfully updated control {controlName} to {updateDto.State}"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error updating secure score control"
            );
        }
    }
}
