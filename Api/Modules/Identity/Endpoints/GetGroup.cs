using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Identity.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Identity;
using DispatchR;

namespace CIPP.Api.Modules.Identity.Endpoints;

public static class GetGroup {
    public static void MapGetGroup(this RouteGroupBuilder group) {
        group.MapGet("/{id}", Handle)
            .WithName("GetGroup")
            .WithSummary("Get group")
            .WithDescription("Retrieves a specific group by ID")
            .RequirePermission("Identity.Group.Read", "View groups");
    }

    private static async Task<IResult> Handle(
        string id,
        string tenantId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetGroupQuery(tenantId, id);
            var result = await mediator.Send(query, cancellationToken);

            if (result == null) {
                return Results.NotFound(Response<GroupDto>.ErrorResult("Group not found"));
            }

            return Results.Ok(Response<GroupDto>.SuccessResult(result, "Group retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving group"
            );
        }
    }
}