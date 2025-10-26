using CIPP.Api.Extensions;
using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.SafePolicies;

public static class GetSafeAttachmentPolicy {
    public static void MapGetSafeAttachmentPolicy(this RouteGroupBuilder group) {
        group.MapGet("/{policyName}", Handle)
            .WithName("GetSafeAttachmentPolicy")
            .WithSummary("Get Safe Attachments policy")
            .WithDescription("Retrieves a specific Safe Attachments policy")
            .RequirePermission("Exchange.SafeAttachments.Read", "Get Safe Attachments policy");
    }

    private static async Task<IResult> Handle(
        HttpContext context,
        Guid tenantId,
        string policyName,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetSafeAttachmentPolicyQuery(tenantId, policyName);
            var result = await mediator.Send(query, cancellationToken);

            return Results.Ok(Response<SafeAttachmentsPolicyDto>.SuccessResult(result, "Get Safe Attachments policy successful"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error with get safe attachments policy"
            );
        }
    }
}
