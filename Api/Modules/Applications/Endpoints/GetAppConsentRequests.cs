using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Applications.Queries;
using CIPP.Api.Extensions;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Applications;
using DispatchR;

namespace CIPP.Api.Modules.Applications.Endpoints;

public static class GetAppConsentRequests {
    public static void MapGetAppConsentRequests(this RouteGroupBuilder group) {
        group.MapGet("", Handle)
            .WithName("GetAppConsentRequests")
            .WithSummary("Get all app consent requests")
            .WithDescription("Retrieves all application consent requests for the specified tenant with pagination support")
            .RequirePermission("Applications.Consent.Read", "View app consent requests");
    }

    private static async Task<IResult> Handle(
        HttpContext context,
        string tenantId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var pagingParams = context.GetPagingParameters();
            var query = new GetAppConsentRequestsQuery(tenantId, pagingParams);
            var result = await mediator.Send(query, cancellationToken);

            return Results.Ok(Response<PagedResponse<AppConsentRequestDto>>.SuccessResult(result, "App consent requests retrieved successfully"));
        } catch (Exception ex) {
            return Results.Json(
                Response<PagedResponse<AppConsentRequestDto>>.ErrorResult("Error retrieving app consent requests", new List<string> { ex.Message }),
                statusCode: 500
            );
        }
    }
}
