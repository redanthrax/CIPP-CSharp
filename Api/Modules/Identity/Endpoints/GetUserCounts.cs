using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Identity.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Identity;
using DispatchR;

namespace CIPP.Api.Modules.Identity.Endpoints;

public static class GetUserCounts {
    public static void MapGetUserCounts(this RouteGroupBuilder group) {
        group.MapGet("/counts", Handle)
            .WithName("GetUserCounts")
            .WithSummary("Get user counts")
            .WithDescription("Returns user statistics including licensed users, guests, and global admins")
            .RequirePermission("Identity.User.Read", "View user statistics");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetUserCountsQuery(tenantId);
            var userCounts = await mediator.Send(query, cancellationToken);

            if (userCounts == null) {
                return Results.NotFound(Response<UserCountsDto?>.ErrorResult($"User counts for tenant {tenantId} not found"));
            }

            return Results.Ok(Response<UserCountsDto>.SuccessResult(userCounts, "User counts retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving user counts"
            );
        }
    }
}
