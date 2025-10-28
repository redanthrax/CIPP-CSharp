using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Commands.JournalRules;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.JournalRules;

public static class CreateJournalRule {
    public static void MapCreateJournalRule(this RouteGroupBuilder group) {
        group.MapPost("/journal-rules", Handle)
            .WithName("CreateJournalRule")
            .WithSummary("Create journal rule")
            .WithDescription("Creates a new journal rule for the specified tenant")
            .RequirePermission("Exchange.JournalRules.ReadWrite", "Manage journal rules");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        CreateJournalRuleDto request,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            request.TenantId = tenantId;
            
            var command = new CreateJournalRuleCommand(tenantId, request);
            var result = await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<string>.SuccessResult(result, "Journal rule created successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error creating journal rule"
            );
        }
    }
}
