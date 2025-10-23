using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Identity.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Identity;
using DispatchR;

namespace CIPP.Api.Modules.Identity.Endpoints;

public static class GetUser {
    public static void MapGetUser(this RouteGroupBuilder group) {
        group.MapGet("/{id}", Handle)
            .WithName("GetUser")
            .WithSummary("Get user by ID")
            .WithDescription("Retrieves a specific user by their ID")
            .RequirePermission("Identity.User.Read", "View user details");
    }

    private static async Task<IResult> Handle(
        string id,
        string tenantId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetUserQuery(tenantId, id);
            var result = await mediator.Send(query, cancellationToken);

            if (result == null) {
                return Results.NotFound(Response<UserDto?>.ErrorResult("User not found"));
            }

            return Results.Ok(Response<UserDto>.SuccessResult(result, "User retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving user"
            );
        }
    }
}