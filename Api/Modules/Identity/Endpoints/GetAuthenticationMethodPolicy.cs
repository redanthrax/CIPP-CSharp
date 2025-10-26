using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Identity.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Identity;
using DispatchR;

namespace CIPP.Api.Modules.Identity.Endpoints;

public static class GetAuthenticationMethodPolicy {
    public static void MapGetAuthenticationMethodPolicy(this RouteGroupBuilder group) {
        group.MapGet("/", Handle)
            .WithName("GetAuthenticationMethodPolicy")
            .WithSummary("Get authentication method policy")
            .WithDescription("Retrieves the authentication method policy for a tenant")
            .RequirePermission("Identity.AuthenticationMethod.Read", "View authentication method policy");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetAuthenticationMethodPolicyQuery(tenantId);
            var result = await mediator.Send(query, cancellationToken);

            if (result == null) {
                return Results.NotFound(Response<AuthenticationMethodPolicyDto?>.ErrorResult("Authentication method policy not found"));
            }

            return Results.Ok(Response<AuthenticationMethodPolicyDto>.SuccessResult(result, "Authentication method policy retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving authentication method policy"
            );
        }
    }
}
