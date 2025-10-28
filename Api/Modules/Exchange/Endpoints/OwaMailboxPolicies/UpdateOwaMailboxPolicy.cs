using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Commands.OwaMailboxPolicies;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.OwaMailboxPolicies;

public static class UpdateOwaMailboxPolicy {
    public static void MapUpdateOwaMailboxPolicy(this RouteGroupBuilder group) {
        group.MapPut("/{policyId}", Handle)
            .WithName("UpdateOwaMailboxPolicy")
            .WithSummary("Update OWA mailbox policy")
            .WithDescription("Updates an existing OWA mailbox policy")
            .RequirePermission("Exchange.OwaMailboxPolicies.Write", "Update OWA mailbox policies");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string policyId,
        UpdateOwaMailboxPolicyDto updateDto,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new UpdateOwaMailboxPolicyCommand(tenantId, policyId, updateDto);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<string>.SuccessResult(policyId, "OWA mailbox policy updated successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error updating OWA mailbox policy"
            );
        }
    }
}
