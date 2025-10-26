using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.SharePoint.Commands;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.SharePoint;
using DispatchR;

namespace CIPP.Api.Modules.SharePoint.Endpoints.Teams;

public static class RemoveTeamsPhoneNumber {
    public static void MapRemoveTeamsPhoneNumber(this RouteGroupBuilder group) {
        group.MapPost("/voice/remove", Handle)
            .WithName("RemoveTeamsPhoneNumber")
            .WithSummary("Remove Teams phone number")
            .WithDescription("Removes a phone number assignment from a user")
            .RequirePermission("Teams.Voice.ReadWrite", "Remove phone numbers");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        RemoveTeamsPhoneNumberDto request,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new RemoveTeamsPhoneNumberCommand(tenantId, request);
            var result = await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<string>.SuccessResult(result, "Phone number removed successfully"));
        } catch (InvalidOperationException ex) {
            return Results.BadRequest(Response<string>.ErrorResult(ex.Message));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error removing phone number"
            );
        }
    }
}
