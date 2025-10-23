using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Identity.Commands;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Identity;
using DispatchR;

namespace CIPP.Api.Modules.Identity.Endpoints;

public static class AddGroupMember {
    public static void MapAddGroupMember(this RouteGroupBuilder group) {
        group.MapPost("/{id}/members", Handle)
            .WithName("AddGroupMember")
            .WithSummary("Add group member")
            .WithDescription("Adds a member to a group")
            .RequirePermission("Identity.Group.ReadWrite", "Manage group members");
    }

    private static async Task<IResult> Handle(
        string id,
        string tenantId,
        AddGroupMemberDto memberData,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new AddGroupMemberCommand(tenantId, id, memberData);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<object>.SuccessResult(new object(), "Group member added successfully"));
        } catch (InvalidOperationException ex) {
            return Results.BadRequest(Response<object>.ErrorResult(ex.Message));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error adding group member"
            );
        }
    }
}