using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Applications.Commands;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Applications;
using DispatchR;

namespace CIPP.Api.Modules.Applications.Endpoints;

public static class CreateApplicationCredential {
    public static void MapCreateApplicationCredential(this RouteGroupBuilder group) {
        group.MapPost("/{applicationId}/credentials", Handle)
            .WithName("CreateApplicationCredential")
            .WithSummary("Create application credential")
            .WithDescription("Creates a new password or certificate credential for an application")
            .RequirePermission("Applications.Application.ReadWrite", "Manage application credentials");
    }

    private static async Task<IResult> Handle(
        string tenantId,
        string applicationId,
        CreateApplicationCredentialDto request,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            request.TenantId = tenantId;
            request.ApplicationId = applicationId;

            var command = new CreateApplicationCredentialCommand(request);
            var result = await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<ApplicationCredentialDto>.SuccessResult(result, "Application credential created successfully"));
        } catch (Exception ex) {
            return Results.Json(
                Response<ApplicationCredentialDto>.ErrorResult("Error creating application credential", new List<string> { ex.Message }),
                statusCode: 500
            );
        }
    }
}
