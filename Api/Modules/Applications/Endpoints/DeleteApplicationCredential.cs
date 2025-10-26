using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Applications.Commands;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Applications;
using DispatchR;

namespace CIPP.Api.Modules.Applications.Endpoints;

public static class DeleteApplicationCredential {
    public static void MapDeleteApplicationCredential(this RouteGroupBuilder group) {
        group.MapDelete("/{applicationId}/credentials/{keyId}", Handle)
            .WithName("DeleteApplicationCredential")
            .WithSummary("Delete application credential")
            .WithDescription("Deletes a password or certificate credential from an application")
            .RequirePermission("Applications.Application.ReadWrite", "Manage application credentials");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string applicationId,
        string keyId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var request = new DeleteApplicationCredentialDto {
                TenantId = tenantId,
                ApplicationId = applicationId,
                KeyId = keyId
            };

            var command = new DeleteApplicationCredentialCommand(tenantId, request);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<object?>.SuccessResult(null, "Application credential deleted successfully"));
        } catch (Exception ex) {
            return Results.Json(
                Response<object>.ErrorResult("Error deleting application credential", new List<string> { ex.Message }),
                statusCode: 500
            );
        }
    }
}
