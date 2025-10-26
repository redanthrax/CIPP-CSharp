using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Commands;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.SpamFilters;

public static class UpdateAntiSpamPolicy {
    public static void MapUpdateAntiSpamPolicy(this RouteGroupBuilder group) {
        group.MapPut("/anti-spam/{policyId}", Handle)
            .WithName("UpdateAntiSpamPolicy")
            .WithSummary("Update anti-spam policy")
            .WithDescription("Updates an anti-spam policy for a tenant")
            .RequirePermission("Exchange.SpamFilter.Write", "Manage spam filter policies");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string policyId,
        UpdateHostedContentFilterPolicyDto updateDto,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new UpdateAntiSpamPolicyCommand(tenantId, policyId, updateDto);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<object>.SuccessResult(null, "Anti-spam policy updated successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error updating anti-spam policy"
            );
        }
    }
}
