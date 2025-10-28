using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Commands.MailResources;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.MailResources;

public static class UpdateResourceBookingPolicy {
    public static void MapUpdateResourceBookingPolicy(this RouteGroupBuilder group) {
        group.MapPut("/{tenantId}/mail-resources/{identity}/booking-policy", Handle)
            .WithName("UpdateResourceBookingPolicy")
            .WithSummary("Update resource booking policy")
            .WithDescription("Updates the booking policy for a room or equipment mailbox")
            .RequirePermission("CIPP.Exchange.MailResources.ReadWrite", "Update resource booking policy");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string identity,
        UpdateResourceBookingPolicyDto updateDto,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new UpdateResourceBookingPolicyCommand(tenantId, identity, updateDto);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<string>.SuccessResult("Resource booking policy updated successfully"));
        } catch (InvalidOperationException ex) {
            return Results.BadRequest(Response<string>.ErrorResult(ex.Message));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error updating resource booking policy"
            );
        }
    }
}
