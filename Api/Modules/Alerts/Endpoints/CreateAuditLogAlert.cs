using CIPP.Api.Modules.Alerts.Commands;
using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Alerts;
using DispatchR;

namespace CIPP.Api.Modules.Alerts.Endpoints;

public static class CreateAuditLogAlert {
    public static void MapCreateAuditLogAlert(this RouteGroupBuilder group) {
        group.MapPost("/audit-log", Handle)
            .WithName("CreateAuditLogAlert")
            .WithSummary("Create audit log alert")
            .WithDescription("Creates a new audit log alert configuration using webhook subscriptions")
            .RequirePermission("CIPP.Alert.ReadWrite", "Create audit log alerts");
    }

    private static async Task<IResult> Handle(
        CreateAuditLogAlertDto alertData,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new CreateAuditLogAlertCommand(alertData);
            var result = await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<string>.SuccessResult(result, "Audit log alert created successfully"));
        } catch (InvalidOperationException ex) {
            return Results.BadRequest(Response<string>.ErrorResult(ex.Message));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error creating audit log alert"
            );
        }
    }
}