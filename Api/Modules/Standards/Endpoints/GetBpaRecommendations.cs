using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Standards.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Standards;
using DispatchR;

namespace CIPP.Api.Modules.Standards.Endpoints;

public static class GetBpaRecommendations {
    public static void MapGetBpaRecommendations(this RouteGroupBuilder group) {
        group.MapGet("/{tenantId:guid}/bpa-recommendations", Handle)
            .WithName("GetBpaRecommendations")
            .WithSummary("Get BPA recommendations")
            .WithDescription("Returns BPA recommendations for a tenant, optionally filtered by severity and category")
            .RequirePermission("CIPP.Standards.Read", "View BPA recommendations");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string? severity,
        string? category,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetBpaRecommendationsQuery(tenantId, severity, category);
            var result = await mediator.Send(query, cancellationToken);

            return Results.Ok(Response<List<BpaRecommendationDto>>.SuccessResult(result, "BPA recommendations retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving BPA recommendations"
            );
        }
    }
}
