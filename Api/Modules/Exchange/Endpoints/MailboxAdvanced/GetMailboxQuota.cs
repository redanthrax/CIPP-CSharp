using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.MailboxAdvanced;

public static class GetMailboxQuota {
    public static void MapGetMailboxQuota(this RouteGroupBuilder group) {
        group.MapGet("/quota", Handle)
            .WithName("GetMailboxQuota")
            .WithSummary("Get mailbox quota")
            .WithDescription("Retrieves quota settings for a mailbox")
            .RequirePermission("Exchange.Mailbox.Read", "View mailboxes");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string mailboxId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetMailboxQuotaQuery(tenantId, mailboxId);
            var result = await mediator.Send(query, cancellationToken);

            if (result == null) {
                return Results.NotFound(Response<object>.ErrorResult("Mailbox quota not found"));
            }

            return Results.Ok(Response<MailboxQuotaDto>.SuccessResult(result, "Mailbox quota retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving mailbox quota"
            );
        }
    }
}
