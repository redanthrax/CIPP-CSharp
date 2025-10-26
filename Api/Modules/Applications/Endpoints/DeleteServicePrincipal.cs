using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Applications.Commands;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.Applications.Endpoints;

public static class DeleteServicePrincipal {
    public static void MapDeleteServicePrincipal(this RouteGroupBuilder group) {
        group.MapDelete("/{servicePrincipalId}", Handle)
            .WithName("DeleteServicePrincipal")
            .WithSummary("Delete service principal")
            .WithDescription("Deletes a service principal (enterprise application)")
            .RequirePermission("Applications.ServicePrincipal.ReadWrite", "Delete service principals");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string servicePrincipalId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new DeleteServicePrincipalCommand(tenantId, servicePrincipalId);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<object?>.SuccessResult(null, "Service principal deleted successfully"));
        } catch (Exception ex) {
            return Results.Json(
                Response<object>.ErrorResult("Error deleting service principal", new List<string> { ex.Message }),
                statusCode: 500
            );
        }
    }
}
