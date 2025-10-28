using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Queries.MailResources;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.MailResources;

public static class GetEquipmentMailbox {
    public static void MapGetEquipmentMailbox(this RouteGroupBuilder group) {
        group.MapGet("/{tenantId}/mail-resources/equipment/{identity}", Handle)
            .WithName("GetEquipmentMailbox")
            .WithSummary("Get equipment mailbox")
            .WithDescription("Returns a specific equipment mailbox by identity")
            .RequirePermission("CIPP.Exchange.MailResources.Read", "View equipment mailbox");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string identity,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetEquipmentMailboxQuery(tenantId, identity);
            var result = await mediator.Send(query, cancellationToken);

            if (result == null) {
                return Results.NotFound(Response<EquipmentMailboxDto>.ErrorResult("Equipment mailbox not found"));
            }

            return Results.Ok(Response<EquipmentMailboxDto>.SuccessResult(result, "Equipment mailbox retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving equipment mailbox"
            );
        }
    }
}
