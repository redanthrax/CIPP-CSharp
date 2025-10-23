using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.SharePoint.Commands;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.SharePoint;
using DispatchR;

namespace CIPP.Api.Modules.SharePoint.Endpoints.Teams;

public static class CreateTeam {
    public static void MapCreateTeam(this RouteGroupBuilder group) {
        group.MapPost("/", Handle)
            .WithName("CreateTeam")
            .WithSummary("Create Team")
            .WithDescription("Creates a new Microsoft Team")
            .RequirePermission("Teams.Group.ReadWrite", "Create Team");
    }

    private static async Task<IResult> Handle(
        string tenantId,
        CreateTeamDto request,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new CreateTeamCommand(tenantId, request);
            var result = await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<string>.SuccessResult(result, "Team created successfully"));
        } catch (InvalidOperationException ex) {
            return Results.BadRequest(Response<string>.ErrorResult(ex.Message));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error creating Team"
            );
        }
    }
}
