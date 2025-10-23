using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Security.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Security;
using DispatchR;

namespace CIPP.Api.Modules.Security.Endpoints;

public static class GetSecureScoreControlProfile {
    public static void MapGetSecureScoreControlProfile(this RouteGroupBuilder group) {
        group.MapGet("/control-profiles/{controlName}", Handle)
            .WithName("GetSecureScoreControlProfile")
            .WithSummary("Get a specific Secure Score control profile")
            .WithDescription("Retrieves details for a specific secure score control profile")
            .RequirePermission("Tenant.Administration.Read", "View secure score control profile");
    }

    private static async Task<IResult> Handle(
        string tenantId,
        string controlName,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetSecureScoreControlProfileQuery(tenantId, controlName);
            var result = await mediator.Send(query, cancellationToken);

            if (result == null) {
                return Results.NotFound($"Control profile '{controlName}' not found");
            }

            return Results.Ok(Response<SecureScoreControlProfileDto>.SuccessResult(result, "Control profile retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving secure score control profile"
            );
        }
    }
}
