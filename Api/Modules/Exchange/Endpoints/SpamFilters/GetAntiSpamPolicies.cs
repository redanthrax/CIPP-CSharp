using CIPP.Api.Extensions;
using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.SpamFilters;

public static class GetAntiSpamPolicies {
    public static void MapGetAntiSpamPolicies(this RouteGroupBuilder group) {
        group.MapGet("/anti-spam", Handle)
            .WithName("GetAntiSpamPolicies")
            .WithSummary("Get anti-spam policies")
            .WithDescription("Retrieves all anti-spam (hosted content filter) policies for a tenant with pagination support")
            .RequirePermission("Exchange.SpamFilter.Read", "View spam filter policies");
    }

    private static async Task<IResult> Handle(
        HttpContext context,
        Guid tenantId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var pagingParams = context.GetPagingParameters();
            var query = new GetAntiSpamPoliciesQuery(tenantId, pagingParams);
            var result = await mediator.Send(query, cancellationToken);

            return Results.Ok(Response<PagedResponse<HostedContentFilterPolicyDto>>.SuccessResult(result, "Anti-spam policies retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving anti-spam policies"
            );
        }
    }
}
