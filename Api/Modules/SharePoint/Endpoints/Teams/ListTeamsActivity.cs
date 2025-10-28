using CIPP.Api.Extensions;
using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.SharePoint.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.SharePoint;
using DispatchR;

namespace CIPP.Api.Modules.SharePoint.Endpoints.Teams;

public static class ListTeamsActivity {
    public static void MapListTeamsActivity(this RouteGroupBuilder group) {
        group.MapGet("/activity", Handle)
            .WithName("ListTeamsActivity")
            .WithSummary("List Teams activity")
            .WithDescription("Retrieves Teams user activity report")
            .RequirePermission("Teams.Activity.Read", "View Teams activity");
    }

    private static async Task<IResult> Handle(
        HttpContext context,
        Guid tenantId,
        string type,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var pagingParams = context.GetPagingParameters();
            var query = new GetTeamsActivityQuery(tenantId, type, pagingParams);
            var result = await mediator.Send(query, cancellationToken);

            return Results.Ok(Response<PagedResponse<TeamsActivityDto>>.SuccessResult(result, "Teams activity retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving Teams activity"
            );
        }
    }
}
