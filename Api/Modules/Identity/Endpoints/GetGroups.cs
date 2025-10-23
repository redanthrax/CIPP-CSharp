using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Identity.Queries;
using CIPP.Api.Extensions;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Identity;
using DispatchR;

namespace CIPP.Api.Modules.Identity.Endpoints;

public static class GetGroups {
    public static void MapGetGroups(this RouteGroupBuilder group) {
        group.MapGet("", Handle)
            .WithName("GetGroups")
            .WithSummary("Get all groups")
            .WithDescription("Retrieves all groups for the specified tenant with pagination support")
            .RequirePermission("Identity.Group.Read", "View groups");
    }

    private static async Task<IResult> Handle(
        HttpContext context,
        string tenantId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var pagingParams = context.GetPagingParameters();
            var query = new GetGroupsQuery(tenantId, pagingParams);
            var result = await mediator.Send(query, cancellationToken);

            return Results.Ok(Response<PagedResponse<GroupDto>>.SuccessResult(result, "Groups retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving groups"
            );
        }
    }
}
