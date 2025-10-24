using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.MailboxAdvanced;

public static class GetMailboxStatistics {
    public static void MapGetMailboxStatistics(this RouteGroupBuilder group) {
        group.MapGet("/statistics", Handle)
            .WithName("GetMailboxStatistics")
            .WithSummary("Get mailbox statistics")
            .WithDescription("Retrieves detailed statistics for a mailbox")
            .RequirePermission("Exchange.Mailbox.Read", "View mailboxes");
    }

    private static async Task<IResult> Handle(
        string tenantId,
        string mailboxId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetMailboxStatisticsQuery(tenantId, mailboxId);
            var result = await mediator.Send(query, cancellationToken);

            if (result == null) {
                return Results.NotFound(Response<object>.ErrorResult("Mailbox statistics not found"));
            }

            return Results.Ok(Response<MailboxStatisticsDto>.SuccessResult(result, "Mailbox statistics retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving mailbox statistics"
            );
        }
    }
}
