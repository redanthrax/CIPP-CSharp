using CIPP.Api.Extensions;
using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.SpamFilters;

public static class GetQuarantineMessages {
    public static void MapGetQuarantineMessages(this RouteGroupBuilder group) {
        group.MapGet("/quarantine", Handle)
            .WithName("GetQuarantineMessages")
            .WithSummary("Get quarantine messages")
            .WithDescription("Retrieves quarantine messages for a tenant with pagination support")
            .RequirePermission("Exchange.SpamFilter.Read", "View spam filter policies");
    }

    private static async Task<IResult> Handle(
        HttpContext context,
        string tenantId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var pagingParams = context.GetPagingParameters();
            var query = new GetQuarantineMessagesQuery(tenantId, pagingParams);
            var result = await mediator.Send(query, cancellationToken);

            return Results.Ok(Response<PagedResponse<QuarantineMessageDto>>.SuccessResult(result, "Quarantine messages retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving quarantine messages"
            );
        }
    }
}
