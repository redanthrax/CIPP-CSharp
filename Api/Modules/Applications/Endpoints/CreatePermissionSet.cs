using CIPP.Api.Modules.Applications.Commands;
using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Applications;
using DispatchR;

namespace CIPP.Api.Modules.Applications.Endpoints;

public static class CreatePermissionSet {
    public static void MapCreatePermissionSet(this RouteGroupBuilder group) {
        group.MapPost("/permission-sets", Handle)
            .WithName("CreatePermissionSet")
            .WithSummary("Create a permission set")
            .WithDescription("Creates a new permission set")
            .RequirePermission("Applications.PermissionSet.Write", "Create permission sets");
    }

    private static async Task<IResult> Handle(
        CreatePermissionSetDto createDto,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new CreatePermissionSetCommand(createDto);
            var result = await mediator.Send(command, cancellationToken);

            return Results.Created($"/api/applications/permission-sets/{result.Id}", Response<PermissionSetDto>.SuccessResult(result, "Permission set created successfully"));
        } catch (Exception ex) {
            return Results.Json(
                Response<PermissionSetDto>.ErrorResult("Error creating permission set", new List<string> { ex.Message }),
                statusCode: 500
            );
        }
    }
}
