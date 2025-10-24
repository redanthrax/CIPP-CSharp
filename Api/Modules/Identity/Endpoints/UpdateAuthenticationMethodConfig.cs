using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Identity.Commands;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Identity;
using DispatchR;

namespace CIPP.Api.Modules.Identity.Endpoints;

public static class UpdateAuthenticationMethodConfig {
    public static void MapUpdateAuthenticationMethodConfig(this RouteGroupBuilder group) {
        group.MapPut("/{methodId}", Handle)
            .WithName("UpdateAuthenticationMethodConfig")
            .WithSummary("Update authentication method configuration")
            .WithDescription("Updates a specific authentication method configuration")
            .RequirePermission("Identity.AuthenticationMethod.Write", "Update authentication method configuration");
    }

    private static async Task<IResult> Handle(
        string methodId,
        string tenantId,
        UpdateAuthenticationMethodDto updateDto,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new UpdateAuthenticationMethodConfigCommand(tenantId, methodId, updateDto);
            var result = await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<AuthenticationMethodDto>.SuccessResult(result, "Authentication method configuration updated successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error updating authentication method configuration"
            );
        }
    }
}
