using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Security.Commands;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Security;
using DispatchR;

namespace CIPP.Api.Modules.Security.Endpoints;

public static class UpdateSecurityIncident {
    public static void MapUpdateSecurityIncident(this RouteGroupBuilder group) {
        group.MapPatch("/incidents/{incidentId}", Handle)
            .WithName("UpdateSecurityIncident")
            .WithSummary("Update a security incident")
            .WithDescription("Updates properties of a specific security incident")
            .RequirePermission("Security.Incident.Write", "Update security incidents");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string incidentId,
        UpdateSecurityIncidentDto updateDto,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            updateDto.TenantId = tenantId;
            var command = new UpdateSecurityIncidentCommand(tenantId, incidentId, updateDto);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<string>.SuccessResult("Incident updated successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error updating security incident"
            );
        }
    }
}
