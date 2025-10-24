using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Queries.MessageTrace;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.MessageTrace;

public static class GetMessageTrace {
    public static void MapGetMessageTrace(this RouteGroupBuilder group) {
        group.MapGet("/", Handle)
            .WithName("GetMessageTrace")
            .WithSummary("Get message trace")
            .WithDescription("Traces messages for troubleshooting delivery issues")
            .RequirePermission("Exchange.MessageTrace.Read", "View message trace");
    }

    private static async Task<IResult> Handle(
        string tenantId,
        [AsParameters] PagingParameters pagingParams,
        [AsParameters] MessageTraceSearchDto searchDto,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetMessageTraceQuery(tenantId, searchDto, pagingParams);
            var result = await mediator.Send(query, cancellationToken);
            return Results.Ok(result);
        } catch (Exception ex) {
            return Results.Problem(detail: ex.Message, statusCode: 500, title: "Error retrieving message trace");
        }
    }
}
