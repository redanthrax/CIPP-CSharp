using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Identity.Commands;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.Identity.Endpoints;

public static class DeleteGroup {
    public static void MapDeleteGroup(this RouteGroupBuilder group) {
        group.MapDelete("/{id}", Handle)
            .WithName("DeleteGroup")
            .WithSummary("Delete group")
            .WithDescription("Deletes a group from the specified tenant")
            .RequirePermission("Identity.Group.ReadWrite", "Delete groups");
    }

    private static async Task<IResult> Handle(
        string id,
        string tenantId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new DeleteGroupCommand(tenantId, id);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<object>.SuccessResult(new object(), "Group deleted successfully"));
        } catch (InvalidOperationException ex) {
            return Results.BadRequest(Response<object>.ErrorResult(ex.Message));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error deleting group"
            );
        }
    }
}