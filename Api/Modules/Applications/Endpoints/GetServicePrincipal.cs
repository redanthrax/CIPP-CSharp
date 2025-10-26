using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Applications.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Applications;
using DispatchR;

namespace CIPP.Api.Modules.Applications.Endpoints;

public static class GetServicePrincipal {
    public static void MapGetServicePrincipal(this RouteGroupBuilder group) {
        group.MapGet("/{servicePrincipalId}", Handle)
            .WithName("GetServicePrincipal")
            .WithSummary("Get service principal by ID")
            .WithDescription("Retrieves a specific service principal by ID")
            .RequirePermission("Applications.ServicePrincipal.Read", "View service principals");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string servicePrincipalId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetServicePrincipalQuery(tenantId, servicePrincipalId);
            var result = await mediator.Send(query, cancellationToken);

            if (result == null) {
                return Results.NotFound(Response<ServicePrincipalDto>.ErrorResult("Service principal not found", new List<string> { $"Service principal {servicePrincipalId} not found" }));
            }

            return Results.Ok(Response<ServicePrincipalDto>.SuccessResult(result, "Service principal retrieved successfully"));
        } catch (Exception ex) {
            return Results.Json(
                Response<ServicePrincipalDto>.ErrorResult("Error retrieving service principal", new List<string> { ex.Message }),
                statusCode: 500
            );
        }
    }
}
