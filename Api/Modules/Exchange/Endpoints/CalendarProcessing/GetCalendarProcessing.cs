using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Queries.CalendarProcessing;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.CalendarProcessing;

public static class GetCalendarProcessing {
    public static void MapGetCalendarProcessing(this RouteGroupBuilder group) {
        group.MapGet("/{mailboxIdentity}/calendar-processing", Handle)
            .WithName("GetCalendarProcessing")
            .WithSummary("Get calendar processing settings")
            .WithDescription("Retrieves calendar processing settings for a room or resource mailbox")
            .RequirePermission("Exchange.Mailbox.Read", "View mailbox settings");
    }

    private static async Task<IResult> Handle(
        string tenantId,
        string mailboxIdentity,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetCalendarProcessingQuery(tenantId, mailboxIdentity);
            var result = await mediator.Send(query, cancellationToken);
            
            if (result == null) {
                return Results.NotFound(Response<CalendarProcessingDto>.ErrorResult("Calendar processing settings not found"));
            }

            return Results.Ok(Response<CalendarProcessingDto>.SuccessResult(result, "Calendar processing settings retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(detail: ex.Message, statusCode: 500, title: "Error retrieving calendar processing settings");
        }
    }
}
