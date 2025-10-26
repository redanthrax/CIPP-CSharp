using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Queries.MessageTrace;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.MessageTrace;

public static class GetMessageTraceDetail {
    public static void MapGetMessageTraceDetail(this RouteGroupBuilder group) {
        group.MapGet("/{messageTraceId}/details", Handle)
            .WithName("GetMessageTraceDetail")
            .WithSummary("Get message trace details")
            .WithDescription("Retrieves detailed event information for a specific message trace")
            .RequirePermission("Exchange.MessageTrace.Read", "View message trace");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string messageTraceId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetMessageTraceDetailQuery(tenantId, messageTraceId);
            var result = await mediator.Send(query, cancellationToken);
            return Results.Ok(Response<List<MessageTraceDetailDto>>.SuccessResult(result, "Message trace details retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(detail: ex.Message, statusCode: 500, title: "Error retrieving message trace details");
        }
    }
}
