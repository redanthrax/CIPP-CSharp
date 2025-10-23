using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Intune.Commands;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Intune;
using DispatchR;

namespace CIPP.Api.Modules.Intune.Endpoints;

public static class UpdateIntunePolicy {
    public static void MapUpdateIntunePolicy(this RouteGroupBuilder group) {
        group.MapPut("/{policyId}", Handle)
            .WithName("UpdateIntunePolicy")
            .WithSummary("Update Intune policy")
            .WithDescription("Updates an existing Intune device management policy")
            .RequirePermission("Endpoint.MEM.ReadWrite", "Update Intune policy");
    }

    private static async Task<IResult> Handle(
        string tenantId,
        string policyId,
        UpdateIntunePolicyDto request,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new UpdateIntunePolicyCommand(tenantId, policyId, request);
            var result = await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<IntunePolicyDto>.SuccessResult(result, "Policy updated successfully"));
        } catch (NotSupportedException ex) {
            return Results.BadRequest(Response<IntunePolicyDto>.ErrorResult(ex.Message));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error updating Intune policy"
            );
        }
    }
}
