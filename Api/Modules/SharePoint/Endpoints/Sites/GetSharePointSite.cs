using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.SharePoint.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.SharePoint;
using DispatchR;

namespace CIPP.Api.Modules.SharePoint.Endpoints.Sites;

public static class GetSharePointSite {
    public static void MapGetSharePointSite(this RouteGroupBuilder group) {
        group.MapGet("/{siteId}", Handle)
            .WithName("GetSharePointSite")
            .WithSummary("Get SharePoint site")
            .WithDescription("Retrieves a specific SharePoint site by ID")
            .RequirePermission("Sharepoint.Site.Read", "View SharePoint site");
    }

    private static async Task<IResult> Handle(
        string tenantId,
        string siteId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetSharePointSiteQuery(tenantId, siteId);
            var result = await mediator.Send(query, cancellationToken);

            if (result == null) {
                return Results.NotFound(Response<SharePointSiteDto>.ErrorResult("SharePoint site not found"));
            }

            return Results.Ok(Response<SharePointSiteDto>.SuccessResult(result, "SharePoint site retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving SharePoint site"
            );
        }
    }
}
