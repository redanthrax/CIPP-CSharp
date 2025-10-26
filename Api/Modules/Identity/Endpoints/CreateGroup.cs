using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Identity.Commands;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Identity;
using DispatchR;

namespace CIPP.Api.Modules.Identity.Endpoints;

public static class CreateGroup {
    public static void MapCreateGroup(this RouteGroupBuilder group) {
        group.MapPost("", Handle)
            .WithName("CreateGroup")
            .WithSummary("Create new group")
            .WithDescription("Creates a new group in the specified tenant")
            .RequirePermission("Identity.Group.ReadWrite", "Create groups");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        CreateGroupDto groupData,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            groupData.TenantId = tenantId;
            var command = new CreateGroupCommand(tenantId, groupData);
            var result = await mediator.Send(command, cancellationToken);

            return Results.Created($"/api/identity/groups/{result.Id}", 
                Response<GroupDto>.SuccessResult(result, "Group created successfully"));
        } catch (InvalidOperationException ex) {
            return Results.BadRequest(Response<GroupDto>.ErrorResult(ex.Message));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error creating group"
            );
        }
    }
}