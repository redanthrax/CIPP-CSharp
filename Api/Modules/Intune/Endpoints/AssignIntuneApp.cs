using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Intune.Commands;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Intune;
using DispatchR;

namespace CIPP.Api.Modules.Intune.Endpoints;

public static class AssignIntuneApp {
    public static void MapAssignIntuneApp(this RouteGroupBuilder group) {
        group.MapPost("/{appId}/assign", Handle)
            .WithName("AssignIntuneApp")
            .WithSummary("Assign Intune application")
            .WithDescription("Assigns an Intune application to users or devices")
            .RequirePermission("Endpoint.Application.ReadWrite", "Assign Intune application");
    }

    private static async Task<IResult> Handle(
        string tenantId,
        string appId,
        AssignIntuneAppDto request,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new AssignIntuneAppCommand(tenantId, appId, request.AssignTo);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<object?>.SuccessResult(null, $"Application assigned to {request.AssignTo}"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error assigning Intune application"
            );
        }
    }
}
