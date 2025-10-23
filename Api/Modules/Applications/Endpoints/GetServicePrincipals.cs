using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Applications.Queries;
using CIPP.Api.Extensions;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Applications;
using DispatchR;

namespace CIPP.Api.Modules.Applications.Endpoints;

public static class GetServicePrincipals {
    public static void MapGetServicePrincipals(this RouteGroupBuilder group) {
        group.MapGet("", Handle)
            .WithName("GetServicePrincipals")
            .WithSummary("Get all service principals")
            .WithDescription("Retrieves all service principals (enterprise applications) for the specified tenant with pagination support")
            .RequirePermission("Applications.ServicePrincipal.Read", "View service principals");
    }

    private static async Task<IResult> Handle(
        HttpContext context,
        string tenantId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var pagingParams = context.GetPagingParameters();
            var query = new GetServicePrincipalsQuery(tenantId, pagingParams);
            var result = await mediator.Send(query, cancellationToken);

            return Results.Ok(Response<PagedResponse<ServicePrincipalDto>>.SuccessResult(result, "Service principals retrieved successfully"));
        } catch (Exception ex) {
            return Results.Json(
                Response<PagedResponse<ServicePrincipalDto>>.ErrorResult("Error retrieving service principals", new List<string> { ex.Message }),
                statusCode: 500
            );
        }
    }
}
