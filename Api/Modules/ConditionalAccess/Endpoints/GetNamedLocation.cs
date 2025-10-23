using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.ConditionalAccess.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.ConditionalAccess;
using DispatchR;

namespace CIPP.Api.Modules.ConditionalAccess.Endpoints;

public static class GetNamedLocation {
    public static void MapGetNamedLocation(this RouteGroupBuilder group) {
        group.MapGet("/named-locations/{locationId}", Handle)
            .WithName("GetNamedLocation")
            .WithSummary("Get a named location")
            .WithDescription("Retrieves a specific named location by ID")
            .RequirePermission("ConditionalAccess.NamedLocation.Read", "View named locations");
    }

    private static async Task<IResult> Handle(
        string tenantId,
        string locationId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetNamedLocationQuery(tenantId, locationId);
            var result = await mediator.Send(query, cancellationToken);

            if (result == null) {
                return Results.NotFound(Response<NamedLocationDto>.ErrorResult("Named location not found"));
            }

            return Results.Ok(Response<NamedLocationDto>.SuccessResult(result, "Named location retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving named location"
            );
        }
    }
}
