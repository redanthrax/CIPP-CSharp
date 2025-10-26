using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Applications.Commands;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Applications;
using DispatchR;

namespace CIPP.Api.Modules.Applications.Endpoints;

public static class UpdateServicePrincipal {
    public static void MapUpdateServicePrincipal(this RouteGroupBuilder group) {
        group.MapPut("/{servicePrincipalId}", Handle)
            .WithName("UpdateServicePrincipal")
            .WithSummary("Update a service principal")
            .WithDescription("Updates an existing service principal (enterprise application)")
            .RequirePermission("Applications.ServicePrincipal.Write", "Update service principals");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string servicePrincipalId,
        UpdateServicePrincipalDto servicePrincipal,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new UpdateServicePrincipalCommand(tenantId, servicePrincipalId, servicePrincipal);
            var result = await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<ServicePrincipalDto>.SuccessResult(result, "Service principal updated successfully"));
        } catch (Exception ex) {
            return Results.Json(
                Response<ServicePrincipalDto>.ErrorResult("Error updating service principal", new List<string> { ex.Message }),
                statusCode: 500
            );
        }
    }
}
