using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Standards.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Standards;
using DispatchR;

namespace CIPP.Api.Modules.Standards.Endpoints;

public static class GetBpaReport {
    public static void MapGetBpaReport(this RouteGroupBuilder group) {
        group.MapGet("/{tenantId:guid}/bpa-report", Handle)
            .WithName("GetBpaReport")
            .WithSummary("Get BPA report")
            .WithDescription("Returns a comprehensive Best Practice Analysis report for a tenant")
            .RequirePermission("CIPP.Standards.Read", "View BPA report");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string? category,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetBpaReportQuery(tenantId, category);
            var result = await mediator.Send(query, cancellationToken);

            return Results.Ok(Response<BpaReportDto>.SuccessResult(result, "BPA report generated successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error generating BPA report"
            );
        }
    }
}
