using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Identity.Commands;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Identity;
using DispatchR;

namespace CIPP.Api.Modules.Identity.Endpoints;

public static class CreateBulkUsers {
    public static void MapCreateBulkUsers(this RouteGroupBuilder group) {
        group.MapPost("/bulk", Handle)
            .WithName("CreateBulkUsers")
            .WithSummary("Create multiple users")
            .WithDescription("Creates multiple users in the specified tenant")
            .RequirePermission("Identity.User.ReadWrite", "Create users in bulk");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        BulkCreateUserDto bulkUserData,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new CreateBulkUsersCommand(tenantId, bulkUserData);
            var results = await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<List<BulkUserResultDto>>.SuccessResult(
                results, 
                $"Processed {results.Count} users"));
                
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error creating users"
            );
        }
    }
}