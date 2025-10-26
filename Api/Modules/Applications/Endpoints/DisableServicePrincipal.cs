using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Applications.Commands;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.Applications.Endpoints;

public static class DisableServicePrincipal {
    public static void MapDisableServicePrincipal(this RouteGroupBuilder group) {
        group.MapPost("/{servicePrincipalId}/disable", Handle)
            .WithName("DisableServicePrincipal")
            .WithSummary("Disable service principal")
            .WithDescription("Disables a service principal (enterprise application)")
            .RequirePermission("Applications.ServicePrincipal.ReadWrite", "Manage service principals");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string servicePrincipalId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new DisableServicePrincipalCommand(tenantId, servicePrincipalId);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<object?>.SuccessResult(null, "Service principal disabled successfully"));
        } catch (Exception ex) {
            return Results.Json(
                Response<object>.ErrorResult("Error disabling service principal", new List<string> { ex.Message }),
                statusCode: 500
            );
        }
    }
}
