using CIPP.Api.Modules.Alerts.Commands;
using DispatchR;
using Microsoft.AspNetCore.Mvc;

namespace CIPP.Api.Modules.Alerts.Endpoints;

public static class ReceiveGraphWebhook {
    public static void MapReceiveGraphWebhook(this RouteGroupBuilder group) {
        group.MapPost("/webhooks/graph", Handle)
            .WithName("ReceiveGraphWebhook")
            .WithSummary("Receive Microsoft Graph webhook notification")
            .WithDescription("Endpoint for receiving Microsoft Graph webhook notifications for audit logs")
            .AllowAnonymous();
    }

    private static async Task<IResult> Handle(
        HttpContext context,
        IMediator mediator,
        [FromQuery] string? validationToken = null,
        CancellationToken cancellationToken = default) {
        if (!string.IsNullOrEmpty(validationToken)) {
            return Results.Ok(validationToken);
        }

        try {
            using var reader = new StreamReader(context.Request.Body);
            var body = await reader.ReadToEndAsync(cancellationToken);

            var command = new ReceiveGraphWebhookCommand(body);
            await mediator.Send(command, cancellationToken);

            return Results.Accepted();
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error processing webhook notification"
            );
        }
    }
}
