using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.ConditionalAccess.Queries;
using CIPP.Api.Extensions;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.ConditionalAccess;
using DispatchR;

namespace CIPP.Api.Modules.ConditionalAccess.Endpoints;

public static class GetNamedLocations {
    public static void MapGetNamedLocations(this RouteGroupBuilder group) {
        group.MapGet("/named-locations", Handle)
            .WithName("GetNamedLocations")
            .WithSummary("Get all named locations")
            .WithDescription("Retrieves all named locations for the specified tenant with pagination support")
            .RequirePermission("ConditionalAccess.NamedLocation.Read", "View named locations");
    }

    private static async Task<IResult> Handle(
        HttpContext context,
        string tenantId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var pagingParams = context.GetPagingParameters();
            var query = new GetNamedLocationsQuery(tenantId, pagingParams);
            var result = await mediator.Send(query, cancellationToken);

            return Results.Ok(Response<PagedResponse<NamedLocationDto>>.SuccessResult(result, "Named locations retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving named locations"
            );
        }
    }
}
