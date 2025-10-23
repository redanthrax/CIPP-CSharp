using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Applications.Commands;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.Applications.Endpoints;

public static class DeleteApplication {
    public static void MapDeleteApplication(this RouteGroupBuilder group) {
        group.MapDelete("/{applicationId}", Handle)
            .WithName("DeleteApplication")
            .WithSummary("Delete application")
            .WithDescription("Deletes an application registration")
            .RequirePermission("Applications.Application.ReadWrite", "Delete applications");
    }

    private static async Task<IResult> Handle(
        string tenantId,
        string applicationId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new DeleteApplicationCommand(tenantId, applicationId);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<object?>.SuccessResult(null, "Application deleted successfully"));
        } catch (Exception ex) {
            return Results.Json(
                Response<object>.ErrorResult("Error deleting application", new List<string> { ex.Message }),
                statusCode: 500
            );
        }
    }
}
