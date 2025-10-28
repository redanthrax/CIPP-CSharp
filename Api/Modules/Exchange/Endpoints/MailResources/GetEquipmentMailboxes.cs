using CIPP.Api.Extensions;
using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Queries.MailResources;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.MailResources;

public static class GetEquipmentMailboxes {
    public static void MapGetEquipmentMailboxes(this RouteGroupBuilder group) {
        group.MapGet("/{tenantId}/mail-resources/equipment", Handle)
            .WithName("GetEquipmentMailboxes")
            .WithSummary("Get equipment mailboxes")
            .WithDescription("Returns a paginated list of equipment mailboxes for a tenant")
            .RequirePermission("CIPP.Exchange.MailResources.Read", "View equipment mailboxes");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        HttpContext context,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var pagingParams = context.GetPagingParameters();
            var query = new GetEquipmentMailboxesQuery(tenantId, pagingParams);
            var result = await mediator.Send(query, cancellationToken);

            return Results.Ok(Response<PagedResponse<EquipmentMailboxDto>>.SuccessResult(result, "Equipment mailboxes retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving equipment mailboxes"
            );
        }
    }
}
