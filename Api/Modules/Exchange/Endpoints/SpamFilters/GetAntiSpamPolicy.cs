using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.SpamFilters;

public static class GetAntiSpamPolicy {
    public static void MapGetAntiSpamPolicy(this RouteGroupBuilder group) {
        group.MapGet("/anti-spam/{policyId}", Handle)
            .WithName("GetAntiSpamPolicy")
            .WithSummary("Get anti-spam policy")
            .WithDescription("Retrieves a specific anti-spam policy for a tenant")
            .RequirePermission("Exchange.SpamFilter.Read", "View spam filter policies");
    }

    private static async Task<IResult> Handle(
        string tenantId,
        string policyId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetAntiSpamPolicyQuery(tenantId, policyId);
            var result = await mediator.Send(query, cancellationToken);

            if (result == null) {
                return Results.NotFound(Response<object>.ErrorResult("Anti-spam policy not found"));
            }

            return Results.Ok(Response<HostedContentFilterPolicyDto>.SuccessResult(result, "Anti-spam policy retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving anti-spam policy"
            );
        }
    }
}
