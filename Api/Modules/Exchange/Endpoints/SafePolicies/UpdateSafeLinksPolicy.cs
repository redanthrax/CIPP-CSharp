using CIPP.Api.Extensions;
using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Commands;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.SafePolicies;

public static class UpdateSafeLinksPolicy {
    public static void MapUpdateSafeLinksPolicy(this RouteGroupBuilder group) {
        group.MapPut("/{policyName}", Handle)
            .WithName("UpdateSafeLinksPolicy")
            .WithSummary("Update Safe Links policy")
            .WithDescription("Updates a Safe Links policy")
            .RequirePermission("Exchange.SafeLinks.Write", "Update Safe Links policy");
    }

    private static async Task<IResult> Handle(
        HttpContext context,
        Guid tenantId,
        string policyName,
        UpdateSafeLinksPolicyDto updateDto,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new UpdateSafeLinksPolicyCommand(tenantId, policyName, updateDto);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<object>.SuccessResult(null, "Update Safe Links policy successful"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error with update safe links policy"
            );
        }
    }
}
