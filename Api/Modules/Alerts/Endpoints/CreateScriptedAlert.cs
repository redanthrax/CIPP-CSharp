using CIPP.Api.Modules.Alerts.Commands;
using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Alerts;
using DispatchR;

namespace CIPP.Api.Modules.Alerts.Endpoints;

public static class CreateScriptedAlert {
    public static void MapCreateScriptedAlert(this RouteGroupBuilder group) {
        group.MapPost("/scripted", Handle)
            .WithName("CreateScriptedAlert")
            .WithSummary("Create scripted alert")
            .WithDescription("Creates a new scripted alert configuration with recurring execution")
            .RequirePermission("CIPP.Alert.ReadWrite", "Create scripted alerts");
    }

    private static async Task<IResult> Handle(
        CreateScriptedAlertDto alertData,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new CreateScriptedAlertCommand(alertData);
            var result = await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<string>.SuccessResult(result, "Scripted alert created successfully"));
        } catch (InvalidOperationException ex) {
            return Results.BadRequest(Response<string>.ErrorResult(ex.Message));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error creating scripted alert"
            );
        }
    }
}