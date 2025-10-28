using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Standards.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Standards;
using DispatchR;

namespace CIPP.Api.Modules.Standards.Endpoints;

public static class GetComplianceScore {
    public static void MapGetComplianceScore(this RouteGroupBuilder group) {
        group.MapGet("/{tenantId:guid}/compliance-score", Handle)
            .WithName("GetComplianceScore")
            .WithSummary("Get compliance score")
            .WithDescription("Returns a compliance score with grade for a tenant")
            .RequirePermission("CIPP.Standards.Read", "View compliance score");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetComplianceScoreQuery(tenantId);
            var result = await mediator.Send(query, cancellationToken);

            return Results.Ok(Response<ComplianceScoreDto>.SuccessResult(result, "Compliance score calculated successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error calculating compliance score"
            );
        }
    }
}
