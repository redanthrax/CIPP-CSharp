using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Identity.Commands;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.Identity.Endpoints;

public static class EnableUserMfa {
    public static void MapEnableUserMfa(this RouteGroupBuilder group) {
        group.MapPost("/{id}/enable-mfa", Handle)
            .WithName("EnableUserMfa")
            .WithSummary("Enable MFA for user")
            .WithDescription("Enables Multi-Factor Authentication for a user")
            .RequirePermission("Identity.User.ReadWrite", "Enable user MFA");
    }

    private static async Task<IResult> Handle(
        string id,
        string tenantId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new EnableUserMfaCommand(tenantId, id);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<object>.SuccessResult(new object(), "User MFA enabled successfully"));
        } catch (InvalidOperationException ex) {
            return Results.BadRequest(Response<object>.ErrorResult(ex.Message));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error enabling user MFA"
            );
        }
    }
}