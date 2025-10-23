using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.ConditionalAccess.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.ConditionalAccess;
using DispatchR;

namespace CIPP.Api.Modules.ConditionalAccess.Endpoints;

public static class GetConditionalAccessPolicy {
    public static void MapGetConditionalAccessPolicy(this RouteGroupBuilder group) {
        group.MapGet("/policies/{policyId}", Handle)
            .WithName("GetConditionalAccessPolicy")
            .WithSummary("Get a conditional access policy")
            .WithDescription("Retrieves a specific conditional access policy by ID")
            .RequirePermission("ConditionalAccess.Policy.Read", "View conditional access policies");
    }

    private static async Task<IResult> Handle(
        string tenantId,
        string policyId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetConditionalAccessPolicyQuery(tenantId, policyId);
            var result = await mediator.Send(query, cancellationToken);

            if (result == null) {
                return Results.NotFound(Response<ConditionalAccessPolicyDto>.ErrorResult("Policy not found"));
            }

            return Results.Ok(Response<ConditionalAccessPolicyDto>.SuccessResult(result, "Policy retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving conditional access policy"
            );
        }
    }
}
