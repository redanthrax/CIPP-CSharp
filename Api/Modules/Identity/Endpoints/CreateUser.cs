using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Identity.Commands;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Identity;
using DispatchR;

namespace CIPP.Api.Modules.Identity.Endpoints;

public static class CreateUser {
    public static void MapCreateUser(this RouteGroupBuilder group) {
        group.MapPost("", Handle)
            .WithName("CreateUser")
            .WithSummary("Create new user")
            .WithDescription("Creates a new user in the specified tenant")
            .RequirePermission("Identity.User.ReadWrite", "Create users");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        CreateUserDto userData,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            userData.TenantId = tenantId;
            var command = new CreateUserCommand(tenantId, userData);
            var result = await mediator.Send(command, cancellationToken);

            return Results.Created($"/api/identity/users/{result.Id}", 
                Response<UserDto>.SuccessResult(result, "User created successfully"));
        } catch (InvalidOperationException ex) {
            return Results.BadRequest(Response<UserDto>.ErrorResult(ex.Message));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error creating user"
            );
        }
    }
}