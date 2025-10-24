using CIPP.Api.Extensions;
using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.SafePolicies;

public static class GetSafeAttachmentPolicies {
    public static void MapGetSafeAttachmentPolicies(this RouteGroupBuilder group) {
        group.MapGet("/", Handle)
            .WithName("GetSafeAttachmentPolicies")
            .WithSummary("Get Safe Attachments policies")
            .WithDescription("Retrieves all Safe Attachments policies for a tenant")
            .RequirePermission("Exchange.SafeAttachments.Read", "Get Safe Attachments policies");
    }

    private static async Task<IResult> Handle(
        HttpContext context,
        string tenantId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var pagingParams = context.GetPagingParameters();
            var query = new GetSafeAttachmentPoliciesQuery(tenantId, pagingParams);
            var result = await mediator.Send(query, cancellationToken);

            return Results.Ok(Response<PagedResponse<SafeAttachmentsPolicyDto>>.SuccessResult(result, "Get Safe Attachments policies successful"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error with get safe attachments policies"
            );
        }
    }
}
