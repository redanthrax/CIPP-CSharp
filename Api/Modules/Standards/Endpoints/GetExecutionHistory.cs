using CIPP.Api.Extensions;
using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Standards.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Standards;
using DispatchR;

namespace CIPP.Api.Modules.Standards.Endpoints;

public static class GetExecutionHistory {
    public static void MapGetExecutionHistory(this RouteGroupBuilder group) {
        group.MapGet("/executions", Handle)
            .WithName("GetExecutionHistory")
            .WithSummary("Get execution history")
            .WithDescription("Returns execution history for standards")
            .RequirePermission("CIPP.Standards.Read", "View execution history");
    }

    private static async Task<IResult> Handle(
        Guid? templateId,
        Guid? tenantId,
        HttpContext context,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var pagingParams = context.GetPagingParameters();
            var query = new GetExecutionHistoryQuery(templateId, tenantId, pagingParams);
            var result = await mediator.Send(query, cancellationToken);

            return Results.Ok(Response<PagedResponse<StandardExecutionDto>>.SuccessResult(result, "Execution history retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving execution history"
            );
        }
    }
}
