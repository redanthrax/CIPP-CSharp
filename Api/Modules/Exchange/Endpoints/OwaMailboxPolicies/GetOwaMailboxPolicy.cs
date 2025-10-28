using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Queries.OwaMailboxPolicies;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.OwaMailboxPolicies;

public static class GetOwaMailboxPolicy {
    public static void MapGetOwaMailboxPolicy(this RouteGroupBuilder group) {
        group.MapGet("/{policyId}", Handle)
            .WithName("GetOwaMailboxPolicy")
            .WithSummary("Get OWA mailbox policy")
            .WithDescription("Retrieves a specific OWA mailbox policy by ID")
            .RequirePermission("Exchange.OwaMailboxPolicies.Read", "View OWA mailbox policy details");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string policyId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetOwaMailboxPolicyQuery(tenantId, policyId);
            var policy = await mediator.Send(query, cancellationToken);

            if (policy == null) {
                return Results.NotFound(Response<OwaMailboxPolicyDto>.ErrorResult($"OWA mailbox policy '{policyId}' not found"));
            }

            return Results.Ok(Response<OwaMailboxPolicyDto>.SuccessResult(policy, "OWA mailbox policy retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving OWA mailbox policy"
            );
        }
    }
}
