using CIPP.Api.Modules.Applications.Queries;
using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Applications;
using DispatchR;

namespace CIPP.Api.Modules.Applications.Endpoints;

public static class GetPermissionSet {
    public static void MapGetPermissionSet(this RouteGroupBuilder group) {
        group.MapGet("/permission-sets/{id:guid}", Handle)
            .WithName("GetPermissionSet")
            .WithSummary("Get a permission set")
            .WithDescription("Retrieves a specific permission set by ID")
            .RequirePermission("Applications.PermissionSet.Read", "View permission sets");
    }

    private static async Task<IResult> Handle(
        Guid id,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetPermissionSetQuery(id);
            var result = await mediator.Send(query, cancellationToken);

            if (result == null) {
                return Results.NotFound(Response<PermissionSetDto>.ErrorResult("Permission set not found"));
            }

            return Results.Ok(Response<PermissionSetDto>.SuccessResult(result, "Permission set retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving permission set"
            );
        }
    }
}
