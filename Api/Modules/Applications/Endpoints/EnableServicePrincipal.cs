using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Applications.Commands;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.Applications.Endpoints;

public static class EnableServicePrincipal {
    public static void MapEnableServicePrincipal(this RouteGroupBuilder group) {
        group.MapPost("/{servicePrincipalId}/enable", Handle)
            .WithName("EnableServicePrincipal")
            .WithSummary("Enable service principal")
            .WithDescription("Enables a service principal (enterprise application)")
            .RequirePermission("Applications.ServicePrincipal.ReadWrite", "Manage service principals");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string servicePrincipalId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new EnableServicePrincipalCommand(tenantId, servicePrincipalId);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<object?>.SuccessResult(null, "Service principal enabled successfully"));
        } catch (Exception ex) {
            return Results.Json(
                Response<object>.ErrorResult("Error enabling service principal", new List<string> { ex.Message }),
                statusCode: 500
            );
        }
    }
}
