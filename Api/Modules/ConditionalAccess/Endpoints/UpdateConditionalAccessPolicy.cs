using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.ConditionalAccess.Commands;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.ConditionalAccess;
using DispatchR;

namespace CIPP.Api.Modules.ConditionalAccess.Endpoints;

public static class UpdateConditionalAccessPolicy {
    public static void MapUpdateConditionalAccessPolicy(this RouteGroupBuilder group) {
        group.MapPut("/policies/{policyId}", Handle)
            .WithName("UpdateConditionalAccessPolicy")
            .WithSummary("Update a conditional access policy")
            .WithDescription("Updates an existing conditional access policy")
            .RequirePermission("ConditionalAccess.Policy.Write", "Update conditional access policies");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string policyId,
        UpdateConditionalAccessPolicyDto policy,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new UpdateConditionalAccessPolicyCommand(tenantId, policyId, policy);
            var result = await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<ConditionalAccessPolicyDto>.SuccessResult(result, "Policy updated successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error updating conditional access policy"
            );
        }
    }
}
