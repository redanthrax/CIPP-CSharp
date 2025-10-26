using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Identity.Queries;
using CIPP.Api.Extensions;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Identity;
using DispatchR;

namespace CIPP.Api.Modules.Identity.Endpoints;

public static class GetUsers {
    public static void MapGetUsers(this RouteGroupBuilder group) {
        group.MapGet("", Handle)
            .WithName("GetUsers")
            .WithSummary("Get all users")
            .WithDescription("Retrieves all users for the specified tenant with pagination support")
            .RequirePermission("Identity.User.Read", "View users");
    }

    private static async Task<IResult> Handle(
        HttpContext context,
        Guid tenantId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var pagingParams = context.GetPagingParameters();
            var query = new GetUsersQuery(tenantId, pagingParams);
            var result = await mediator.Send(query, cancellationToken);

            return Results.Ok(Response<PagedResponse<UserDto>>.SuccessResult(result, "Users retrieved successfully"));
        } catch (Exception ex) {
            return Results.Json(
                Response<PagedResponse<UserDto>>.ErrorResult("Error retrieving users", new List<string> { ex.Message }),
                statusCode: 500
            );
        }
    }
}
