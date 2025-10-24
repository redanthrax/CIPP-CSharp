using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Commands.CalendarProcessing;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.CalendarProcessing;

public static class UpdateCalendarProcessing {
    public static void MapUpdateCalendarProcessing(this RouteGroupBuilder group) {
        group.MapPut("/{mailboxIdentity}/calendar-processing", Handle)
            .WithName("UpdateCalendarProcessing")
            .WithSummary("Update calendar processing settings")
            .WithDescription("Updates calendar processing settings for a room or resource mailbox")
            .RequirePermission("Exchange.Mailbox.Write", "Update mailbox settings");
    }

    private static async Task<IResult> Handle(
        string tenantId,
        string mailboxIdentity,
        UpdateCalendarProcessingDto dto,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new UpdateCalendarProcessingCommand(tenantId, mailboxIdentity, dto);
            await mediator.Send(command, cancellationToken);
            return Results.Ok(Response<object>.SuccessResult(null, "Calendar processing settings updated successfully"));
        } catch (Exception ex) {
            return Results.Problem(detail: ex.Message, statusCode: 500, title: "Error updating calendar processing settings");
        }
    }
}
