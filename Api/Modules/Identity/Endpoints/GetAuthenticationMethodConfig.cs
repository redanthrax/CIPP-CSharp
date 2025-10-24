using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Identity.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Identity;
using DispatchR;

namespace CIPP.Api.Modules.Identity.Endpoints;

public static class GetAuthenticationMethodConfig {
    public static void MapGetAuthenticationMethodConfig(this RouteGroupBuilder group) {
        group.MapGet("/{methodId}", Handle)
            .WithName("GetAuthenticationMethodConfig")
            .WithSummary("Get authentication method configuration")
            .WithDescription("Retrieves a specific authentication method configuration")
            .RequirePermission("Identity.AuthenticationMethod.Read", "View authentication method configuration");
    }

    private static async Task<IResult> Handle(
        string methodId,
        string tenantId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetAuthenticationMethodConfigQuery(tenantId, methodId);
            var result = await mediator.Send(query, cancellationToken);

            if (result == null) {
                return Results.NotFound(Response<AuthenticationMethodDto?>.ErrorResult("Authentication method configuration not found"));
            }

            return Results.Ok(Response<AuthenticationMethodDto>.SuccessResult(result, "Authentication method configuration retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving authentication method configuration"
            );
        }
    }
}
