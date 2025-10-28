using CIPP.Api.Extensions;
using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.SharePoint.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.SharePoint;
using DispatchR;

namespace CIPP.Api.Modules.SharePoint.Endpoints.Teams;

public static class ListTeams {
    public static void MapListTeams(this RouteGroupBuilder group) {
        group.MapGet("/", Handle)
            .WithName("ListTeams")
            .WithSummary("List Teams")
            .WithDescription("Retrieves all Microsoft Teams for the specified tenant")
            .RequirePermission("Teams.Group.Read", "View Teams");
    }

    private static async Task<IResult> Handle(
        HttpContext context,
        Guid tenantId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var pagingParams = context.GetPagingParameters();
            var query = new GetTeamsQuery(tenantId, pagingParams);
            var result = await mediator.Send(query, cancellationToken);

            return Results.Ok(Response<PagedResponse<TeamDto>>.SuccessResult(result, "Teams retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving Teams"
            );
        }
    }
}
