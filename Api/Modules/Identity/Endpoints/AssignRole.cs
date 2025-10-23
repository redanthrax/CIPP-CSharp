using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Identity.Commands;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Identity;
using DispatchR;

namespace CIPP.Api.Modules.Identity.Endpoints;

public static class AssignRole {
    public static void MapAssignRole(this RouteGroupBuilder group) {
        group.MapPost("/assign", Handle)
            .WithName("AssignRole")
            .WithSummary("Assign a role")
            .WithDescription("Assigns a role to a user")
            .RequirePermission("Identity.Role.ReadWrite", "Assign roles");
    }

    private static async Task<IResult> Handle(
        AssignRoleDto dto,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new AssignRoleCommand(dto);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<string>.SuccessResult("Role assigned successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error assigning role"
            );
        }
    }
}
