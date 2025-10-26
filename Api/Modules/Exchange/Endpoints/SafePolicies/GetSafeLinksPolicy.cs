using CIPP.Api.Extensions;
using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.SafePolicies;

public static class GetSafeLinksPolicy {
    public static void MapGetSafeLinksPolicy(this RouteGroupBuilder group) {
        group.MapGet("/{policyName}", Handle)
            .WithName("GetSafeLinksPolicy")
            .WithSummary("Get Safe Links policy")
            .WithDescription("Retrieves a specific Safe Links policy")
            .RequirePermission("Exchange.SafeLinks.Read", "Get Safe Links policy");
    }

    private static async Task<IResult> Handle(
        HttpContext context,
        Guid tenantId,
        string policyName,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetSafeLinksPolicyQuery(tenantId, policyName);
            var result = await mediator.Send(query, cancellationToken);

            return Results.Ok(Response<SafeLinksPolicyDto>.SuccessResult(result, "Get Safe Links policy successful"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error with get safe links policy"
            );
        }
    }
}
