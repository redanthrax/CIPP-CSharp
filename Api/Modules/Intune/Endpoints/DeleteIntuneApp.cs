using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Intune.Commands;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.Intune.Endpoints;

public static class DeleteIntuneApp {
    public static void MapDeleteIntuneApp(this RouteGroupBuilder group) {
        group.MapDelete("/{appId}", Handle)
            .WithName("DeleteIntuneApp")
            .WithSummary("Delete Intune application")
            .WithDescription("Deletes a mobile application")
            .RequirePermission("Endpoint.Application.ReadWrite", "Delete Intune application");
    }

    private static async Task<IResult> Handle(
        string tenantId,
        string appId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new DeleteIntuneAppCommand(tenantId, appId);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<object?>.SuccessResult(null, "Application deleted successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error deleting Intune application"
            );
        }
    }
}
