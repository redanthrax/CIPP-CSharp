using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Identity.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Identity;
using DispatchR;

namespace CIPP.Api.Modules.Identity.Endpoints;

public static class GetUserMfaStatus {
    public static void MapGetUserMfaStatus(this RouteGroupBuilder group) {
        group.MapGet("/{id}/mfa-status", Handle)
            .WithName("GetUserMfaStatus")
            .WithSummary("Get user MFA status")
            .WithDescription("Retrieves the Multi-Factor Authentication status for a user")
            .RequirePermission("Identity.User.Read", "View user MFA status");
    }

    private static async Task<IResult> Handle(
        string id,
        string tenantId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetUserMfaStatusQuery(tenantId, id);
            var result = await mediator.Send(query, cancellationToken);

            return Results.Ok(Response<UserMfaStatusDto>.SuccessResult(result, "User MFA status retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving user MFA status"
            );
        }
    }
}