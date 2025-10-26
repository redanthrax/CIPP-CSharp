using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Security.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Security;
using DispatchR;

namespace CIPP.Api.Modules.Security.Endpoints;

public static class GetSecurityIncident {
    public static void MapGetSecurityIncident(this RouteGroupBuilder group) {
        group.MapGet("/incidents/{incidentId}", Handle)
            .WithName("GetSecurityIncident")
            .WithSummary("Get a security incident")
            .WithDescription("Retrieves details of a specific security incident")
            .RequirePermission("Security.Incident.Read", "View security incidents");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string incidentId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetSecurityIncidentQuery(tenantId, incidentId);
            var result = await mediator.Send(query, cancellationToken);

            if (result == null) {
                return Results.NotFound(Response<SecurityIncidentDto>.ErrorResult("Incident not found"));
            }

            return Results.Ok(Response<SecurityIncidentDto>.SuccessResult(result, "Incident retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving security incident"
            );
        }
    }
}
