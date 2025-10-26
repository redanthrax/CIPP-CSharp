using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Identity.Commands;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Identity;
using DispatchR;

namespace CIPP.Api.Modules.Identity.Endpoints;

public static class UpdateGroup {
    public static void MapUpdateGroup(this RouteGroupBuilder group) {
        group.MapPut("/{id}", Handle)
            .WithName("UpdateGroup")
            .WithSummary("Update group")
            .WithDescription("Updates an existing group")
            .RequirePermission("Identity.Group.ReadWrite", "Update groups");
    }

    private static async Task<IResult> Handle(
        string id,
        Guid tenantId,
        UpdateGroupDto groupData,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new UpdateGroupCommand(tenantId, id, groupData);
            var result = await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<GroupDto>.SuccessResult(result, "Group updated successfully"));
        } catch (InvalidOperationException ex) {
            return Results.BadRequest(Response<GroupDto>.ErrorResult(ex.Message));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error updating group"
            );
        }
    }
}