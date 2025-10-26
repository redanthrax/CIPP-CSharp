using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.ConditionalAccess.Commands;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.ConditionalAccess.Endpoints;

public static class DeleteConditionalAccessPolicy {
    public static void MapDeleteConditionalAccessPolicy(this RouteGroupBuilder group) {
        group.MapDelete("/policies/{policyId}", Handle)
            .WithName("DeleteConditionalAccessPolicy")
            .WithSummary("Delete a conditional access policy")
            .WithDescription("Deletes an existing conditional access policy")
            .RequirePermission("ConditionalAccess.Policy.Write", "Delete conditional access policies");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string policyId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new DeleteConditionalAccessPolicyCommand(tenantId, policyId);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<string?>.SuccessResult(null, "Policy deleted successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error deleting conditional access policy"
            );
        }
    }
}
