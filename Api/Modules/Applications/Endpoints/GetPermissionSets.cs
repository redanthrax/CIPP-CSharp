using CIPP.Api.Extensions;
using CIPP.Api.Modules.Applications.Queries;
using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Applications;
using DispatchR;

namespace CIPP.Api.Modules.Applications.Endpoints;

public static class GetPermissionSets {
    public static void MapGetPermissionSets(this RouteGroupBuilder group) {
        group.MapGet("/permission-sets", Handle)
            .WithName("GetPermissionSets")
            .WithSummary("Get all permission sets")
            .WithDescription("Retrieves all permission sets with pagination support")
            .RequirePermission("Applications.PermissionSet.Read", "View permission sets");
    }

    private static async Task<IResult> Handle(
        HttpContext context,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var pagingParams = context.GetPagingParameters();
            var query = new GetPermissionSetsQuery(pagingParams);
            var result = await mediator.Send(query, cancellationToken);

            return Results.Ok(Response<PagedResponse<PermissionSetDto>>.SuccessResult(result, "Permission sets retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving permission sets"
            );
        }
    }
}
