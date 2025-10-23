using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Identity.Commands;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.Identity.Endpoints;

public static class RemoveGroupMember {
    public static void MapRemoveGroupMember(this RouteGroupBuilder group) {
        group.MapDelete("/{id}/members/{userId}", Handle)
            .WithName("RemoveGroupMember")
            .WithSummary("Remove group member")
            .WithDescription("Removes a member from a group")
            .RequirePermission("Identity.Group.ReadWrite", "Manage group members");
    }

    private static async Task<IResult> Handle(
        string id,
        string userId,
        string tenantId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new RemoveGroupMemberCommand(tenantId, id, userId);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<object>.SuccessResult(new object(), "Group member removed successfully"));
        } catch (InvalidOperationException ex) {
            return Results.BadRequest(Response<object>.ErrorResult(ex.Message));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error removing group member"
            );
        }
    }
}