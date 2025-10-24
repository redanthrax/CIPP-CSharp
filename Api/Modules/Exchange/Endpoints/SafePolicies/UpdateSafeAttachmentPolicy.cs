using CIPP.Api.Extensions;
using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Commands;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.SafePolicies;

public static class UpdateSafeAttachmentPolicy {
    public static void MapUpdateSafeAttachmentPolicy(this RouteGroupBuilder group) {
        group.MapPut("/{policyName}", Handle)
            .WithName("UpdateSafeAttachmentPolicy")
            .WithSummary("Update Safe Attachments policy")
            .WithDescription("Updates a Safe Attachments policy")
            .RequirePermission("Exchange.SafeAttachments.Write", "Update Safe Attachments policy");
    }

    private static async Task<IResult> Handle(
        HttpContext context,
        string tenantId,
        string policyName,
        UpdateSafeAttachmentsPolicyDto updateDto,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new UpdateSafeAttachmentPolicyCommand(tenantId, policyName, updateDto);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<object>.SuccessResult(null, "Update Safe Attachments policy successful"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error with update safe attachments policy"
            );
        }
    }
}
