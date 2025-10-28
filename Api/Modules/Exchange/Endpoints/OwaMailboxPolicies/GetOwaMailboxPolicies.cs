using CIPP.Api.Extensions;
using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Queries.OwaMailboxPolicies;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.OwaMailboxPolicies;

public static class GetOwaMailboxPolicies {
    public static void MapGetOwaMailboxPolicies(this RouteGroupBuilder group) {
        group.MapGet("/", Handle)
            .WithName("GetOwaMailboxPolicies")
            .WithSummary("Get OWA mailbox policies")
            .WithDescription("Retrieves all OWA mailbox policies for the specified tenant")
            .RequirePermission("Exchange.OwaMailboxPolicies.Read", "View OWA mailbox policies");
    }

    private static async Task<IResult> Handle(
        HttpContext context,
        Guid tenantId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var pagingParams = context.GetPagingParameters();
            var query = new GetOwaMailboxPoliciesQuery(tenantId, pagingParams);
            var policies = await mediator.Send(query, cancellationToken);

            return Results.Ok(Response<PagedResponse<OwaMailboxPolicyDto>>.SuccessResult(policies, "OWA mailbox policies retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving OWA mailbox policies"
            );
        }
    }
}
