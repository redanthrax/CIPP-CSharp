using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Commands.JournalRules;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.JournalRules;

public static class DeleteJournalRule {
    public static void MapDeleteJournalRule(this RouteGroupBuilder group) {
        group.MapDelete("/journal-rules/{ruleName}", Handle)
            .WithName("DeleteJournalRule")
            .WithSummary("Delete journal rule")
            .WithDescription("Deletes a journal rule for the specified tenant")
            .RequirePermission("Exchange.JournalRules.ReadWrite", "Manage journal rules");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string ruleName,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new DeleteJournalRuleCommand(tenantId, ruleName);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<object?>.SuccessResult(null, "Journal rule deleted successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error deleting journal rule"
            );
        }
    }
}
