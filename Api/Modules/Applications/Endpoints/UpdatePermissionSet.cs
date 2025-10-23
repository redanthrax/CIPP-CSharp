using CIPP.Api.Modules.Applications.Commands;
using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Applications;
using DispatchR;

namespace CIPP.Api.Modules.Applications.Endpoints;

public static class UpdatePermissionSet {
    public static void MapUpdatePermissionSet(this RouteGroupBuilder group) {
        group.MapPut("/permission-sets/{id:guid}", Handle)
            .WithName("UpdatePermissionSet")
            .WithSummary("Update a permission set")
            .WithDescription("Updates an existing permission set")
            .RequirePermission("Applications.PermissionSet.Write", "Update permission sets");
    }

    private static async Task<IResult> Handle(
        Guid id,
        UpdatePermissionSetDto updateDto,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new UpdatePermissionSetCommand(id, updateDto);
            var result = await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<PermissionSetDto>.SuccessResult(result, "Permission set updated successfully"));
        } catch (Exception ex) {
            return Results.Json(
                Response<PermissionSetDto>.ErrorResult("Error updating permission set", new List<string> { ex.Message }),
                statusCode: 500
            );
        }
    }
}
