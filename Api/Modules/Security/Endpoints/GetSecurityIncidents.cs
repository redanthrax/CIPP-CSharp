using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Security.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Security;
using DispatchR;

namespace CIPP.Api.Modules.Security.Endpoints;

public static class GetSecurityIncidents {
    public static void MapGetSecurityIncidents(this RouteGroupBuilder group) {
        group.MapGet("/incidents", Handle)
            .WithName("GetSecurityIncidents")
            .WithSummary("Get all security incidents")
            .WithDescription("Retrieves all security incidents for the specified tenant with aggregated counts")
            .RequirePermission("Security.Incident.Read", "View security incidents");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetSecurityIncidentsQuery(tenantId);
            var result = await mediator.Send(query, cancellationToken);

            return Results.Ok(Response<SecurityIncidentsResponseDto>.SuccessResult(result, "Incidents retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving security incidents"
            );
        }
    }
}
