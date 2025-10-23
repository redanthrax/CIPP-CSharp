using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Identity.Queries;
using CIPP.Api.Extensions;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Identity;
using DispatchR;

namespace CIPP.Api.Modules.Identity.Endpoints;

public static class GetRoles {
    public static void MapGetRoles(this RouteGroupBuilder group) {
        group.MapGet("", Handle)
            .WithName("GetRoles")
            .WithSummary("Get all roles")
            .WithDescription("Retrieves all roles for the specified tenant with pagination support")
            .RequirePermission("Identity.Role.Read", "View roles");
    }

    private static async Task<IResult> Handle(
        HttpContext context,
        string tenantId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var pagingParams = context.GetPagingParameters();
            var query = new GetRolesQuery(tenantId, pagingParams);
            var result = await mediator.Send(query, cancellationToken);

            return Results.Ok(Response<PagedResponse<RoleDto>>.SuccessResult(result, "Roles retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving roles"
            );
        }
    }
}
