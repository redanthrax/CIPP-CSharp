using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Intune.Commands;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.Intune.Endpoints;

public static class DeleteIntunePolicy {
    public static void MapDeleteIntunePolicy(this RouteGroupBuilder group) {
        group.MapDelete("/{policyId}", Handle)
            .WithName("DeleteIntunePolicy")
            .WithSummary("Delete Intune policy")
            .WithDescription("Deletes a device management policy")
            .RequirePermission("Endpoint.MEM.ReadWrite", "Delete Intune policy");
    }

    private static async Task<IResult> Handle(
        string tenantId,
        string policyId,
        string urlName,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new DeleteIntunePolicyCommand(tenantId, policyId, urlName);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<object?>.SuccessResult(null, "Policy deleted successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error deleting Intune policy"
            );
        }
    }
}
