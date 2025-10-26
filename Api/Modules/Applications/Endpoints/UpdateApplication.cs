using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Applications.Commands;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Applications;
using DispatchR;

namespace CIPP.Api.Modules.Applications.Endpoints;

public static class UpdateApplication {
    public static void MapUpdateApplication(this RouteGroupBuilder group) {
        group.MapPut("/{applicationId}", Handle)
            .WithName("UpdateApplication")
            .WithSummary("Update an application")
            .WithDescription("Updates an existing application registration")
            .RequirePermission("Applications.Application.Write", "Update applications");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string applicationId,
        UpdateApplicationDto application,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new UpdateApplicationCommand(tenantId, applicationId, application);
            var result = await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<ApplicationDto>.SuccessResult(result, "Application updated successfully"));
        } catch (Exception ex) {
            return Results.Json(
                Response<ApplicationDto>.ErrorResult("Error updating application", new List<string> { ex.Message }),
                statusCode: 500
            );
        }
    }
}
