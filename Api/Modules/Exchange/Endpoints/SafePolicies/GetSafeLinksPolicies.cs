using CIPP.Api.Extensions;
using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.SafePolicies;

public static class GetSafeLinksPolicies {
    public static void MapGetSafeLinksPolicies(this RouteGroupBuilder group) {
        group.MapGet("/", Handle)
            .WithName("GetSafeLinksPolicies")
            .WithSummary("Get Safe Links policies")
            .WithDescription("Retrieves all Safe Links policies for a tenant")
            .RequirePermission("Exchange.SafeLinks.Read", "Get Safe Links policies");
    }

    private static async Task<IResult> Handle(
        HttpContext context,
        string tenantId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var pagingParams = context.GetPagingParameters();
            var query = new GetSafeLinksPoliciesQuery(tenantId, pagingParams);
            var result = await mediator.Send(query, cancellationToken);

            return Results.Ok(Response<PagedResponse<SafeLinksPolicyDto>>.SuccessResult(result, "Get Safe Links policies successful"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error with get safe links policies"
            );
        }
    }
}
