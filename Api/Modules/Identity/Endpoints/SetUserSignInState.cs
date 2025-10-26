using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Identity.Commands;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Identity;
using DispatchR;

namespace CIPP.Api.Modules.Identity.Endpoints;

public static class SetUserSignInState {
    public static void MapSetUserSignInState(this RouteGroupBuilder group) {
        group.MapPost("/signin-state", Handle)
            .WithName("SetUserSignInState")
            .WithSummary("Enable or disable user sign-in")
            .WithDescription("Enables or disables user sign-in state")
            .RequirePermission("Identity.User.ReadWrite", "Manage user sign-in state");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        SetUserSignInStateDto signInStateData,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new SetUserSignInStateCommand(tenantId, signInStateData);
            var result = await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<string>.SuccessResult(result, "User sign-in state updated"));
                
        } catch (InvalidOperationException ex) {
            return Results.BadRequest(Response<string>.ErrorResult(ex.Message));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error setting user sign-in state"
            );
        }
    }
}