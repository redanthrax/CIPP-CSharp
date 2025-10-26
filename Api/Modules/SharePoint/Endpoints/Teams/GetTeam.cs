using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.SharePoint.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.SharePoint;
using DispatchR;

namespace CIPP.Api.Modules.SharePoint.Endpoints.Teams;

public static class GetTeam {
    public static void MapGetTeam(this RouteGroupBuilder group) {
        group.MapGet("/{teamId}", Handle)
            .WithName("GetTeam")
            .WithSummary("Get Team details")
            .WithDescription("Retrieves detailed information about a specific Team")
            .RequirePermission("Teams.Group.Read", "View Team details");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string teamId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetTeamDetailsQuery(tenantId, teamId);
            var result = await mediator.Send(query, cancellationToken);

            if (result == null) {
                return Results.NotFound(Response<TeamDetailsDto>.ErrorResult("Team not found"));
            }

            return Results.Ok(Response<TeamDetailsDto>.SuccessResult(result, "Team details retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving Team details"
            );
        }
    }
}
