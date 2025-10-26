using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.SharePoint.Commands;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.SharePoint;
using DispatchR;

namespace CIPP.Api.Modules.SharePoint.Endpoints.Teams;

public static class AssignTeamsPhoneNumber {
    public static void MapAssignTeamsPhoneNumber(this RouteGroupBuilder group) {
        group.MapPost("/voice/assign", Handle)
            .WithName("AssignTeamsPhoneNumber")
            .WithSummary("Assign Teams phone number")
            .WithDescription("Assigns a phone number to a user or emergency location")
            .RequirePermission("Teams.Voice.ReadWrite", "Assign phone numbers");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        AssignTeamsPhoneNumberDto request,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new AssignTeamsPhoneNumberCommand(tenantId, request);
            var result = await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<string>.SuccessResult(result, "Phone number assignment initiated"));
        } catch (InvalidOperationException ex) {
            return Results.BadRequest(Response<string>.ErrorResult(ex.Message));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error assigning phone number"
            );
        }
    }
}
