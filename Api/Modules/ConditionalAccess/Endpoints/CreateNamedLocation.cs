using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.ConditionalAccess.Commands;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.ConditionalAccess;
using DispatchR;

namespace CIPP.Api.Modules.ConditionalAccess.Endpoints;

public static class CreateNamedLocation {
    public static void MapCreateNamedLocation(this RouteGroupBuilder group) {
        group.MapPost("/named-locations", Handle)
            .WithName("CreateNamedLocation")
            .WithSummary("Create a named location")
            .WithDescription("Creates a new named location for the specified tenant")
            .RequirePermission("ConditionalAccess.NamedLocation.Write", "Create named locations");
    }

    private static async Task<IResult> Handle(
        string tenantId,
        CreateNamedLocationDto location,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            location.TenantId = tenantId;
            var command = new CreateNamedLocationCommand(location);
            var result = await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<NamedLocationDto>.SuccessResult(result, "Named location created successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error creating named location"
            );
        }
    }
}
