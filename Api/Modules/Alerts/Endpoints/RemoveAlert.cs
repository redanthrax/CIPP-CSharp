using CIPP.Api.Modules.Alerts.Commands;
using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.Alerts.Endpoints;

public static class RemoveAlert {
    public static void MapRemoveAlert(this RouteGroupBuilder group) {
        group.MapDelete("/{id}", Handle)
            .WithName("RemoveAlert")
            .WithSummary("Remove alert")
            .WithDescription("Removes an existing alert configuration")
            .RequirePermission("CIPP.Alert.ReadWrite", "Remove alerts");
    }

    private static async Task<IResult> Handle(
        string id,
        string eventType,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new RemoveAlertCommand(id, eventType);
            var result = await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<string>.SuccessResult(result, "Alert removed successfully"));
        } catch (InvalidOperationException ex) {
            return Results.BadRequest(Response<string>.ErrorResult(ex.Message));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error removing alert"
            );
        }
    }
}