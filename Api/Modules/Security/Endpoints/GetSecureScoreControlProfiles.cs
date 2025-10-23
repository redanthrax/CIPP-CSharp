using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Security.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Security;
using DispatchR;

namespace CIPP.Api.Modules.Security.Endpoints;

public static class GetSecureScoreControlProfiles {
    public static void MapGetSecureScoreControlProfiles(this RouteGroupBuilder group) {
        group.MapGet("/control-profiles", Handle)
            .WithName("GetSecureScoreControlProfiles")
            .WithSummary("Get Secure Score control profiles")
            .WithDescription("Retrieves all secure score control profiles for the specified tenant")
            .RequirePermission("Tenant.Administration.Read", "View secure score control profiles");
    }

    private static async Task<IResult> Handle(
        string tenantId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetSecureScoreControlProfilesQuery(tenantId);
            var result = await mediator.Send(query, cancellationToken);

            return Results.Ok(Response<List<SecureScoreControlProfileDto>>.SuccessResult(result, "Control profiles retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving secure score control profiles"
            );
        }
    }
}
