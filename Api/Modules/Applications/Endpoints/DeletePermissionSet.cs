using CIPP.Api.Modules.Applications.Commands;
using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.Applications.Endpoints;

public static class DeletePermissionSet {
    public static void MapDeletePermissionSet(this RouteGroupBuilder group) {
        group.MapDelete("/permission-sets/{id:guid}", Handle)
            .WithName("DeletePermissionSet")
            .WithSummary("Delete a permission set")
            .WithDescription("Deletes an existing permission set")
            .RequirePermission("Applications.PermissionSet.Write", "Delete permission sets");
    }

    private static async Task<IResult> Handle(
        Guid id,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new DeletePermissionSetCommand(id);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<string>.SuccessResult("Permission set deleted successfully"));
        } catch (Exception ex) {
            return Results.Json(
                Response<string>.ErrorResult("Error deleting permission set", new List<string> { ex.Message }),
                statusCode: 500
            );
        }
    }
}
