using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Identity.Commands;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.Identity.Endpoints;

public static class DisableUserMfa {
    public static void MapDisableUserMfa(this RouteGroupBuilder group) {
        group.MapPost("/{id}/disable-mfa", Handle)
            .WithName("DisableUserMfa")
            .WithSummary("Disable MFA for user")
            .WithDescription("Disables Multi-Factor Authentication for a user")
            .RequirePermission("Identity.User.ReadWrite", "Disable user MFA");
    }

    private static async Task<IResult> Handle(
        string id,
        Guid tenantId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new DisableUserMfaCommand(tenantId, id);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<object>.SuccessResult(new object(), "User MFA disabled successfully"));
        } catch (InvalidOperationException ex) {
            return Results.BadRequest(Response<object>.ErrorResult(ex.Message));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error disabling user MFA"
            );
        }
    }
}