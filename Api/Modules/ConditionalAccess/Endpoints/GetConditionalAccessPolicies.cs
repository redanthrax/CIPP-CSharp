using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.ConditionalAccess.Queries;
using CIPP.Api.Extensions;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.ConditionalAccess;
using DispatchR;

namespace CIPP.Api.Modules.ConditionalAccess.Endpoints;

public static class GetConditionalAccessPolicies {
    public static void MapGetConditionalAccessPolicies(this RouteGroupBuilder group) {
        group.MapGet("/policies", Handle)
            .WithName("GetConditionalAccessPolicies")
            .WithSummary("Get all conditional access policies")
            .WithDescription("Retrieves all conditional access policies for the specified tenant with pagination support")
            .RequirePermission("ConditionalAccess.Policy.Read", "View conditional access policies");
    }

    private static async Task<IResult> Handle(
        HttpContext context,
        string tenantId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var pagingParams = context.GetPagingParameters();
            var query = new GetConditionalAccessPoliciesQuery(tenantId, pagingParams);
            var result = await mediator.Send(query, cancellationToken);

            return Results.Ok(Response<PagedResponse<ConditionalAccessPolicyDto>>.SuccessResult(result, "Policies retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving conditional access policies"
            );
        }
    }
}
