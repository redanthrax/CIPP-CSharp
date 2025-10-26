using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.SharePoint.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.SharePoint;
using DispatchR;

namespace CIPP.Api.Modules.SharePoint.Endpoints.Sites;

public static class ListSharePointSites {
    public static void MapListSharePointSites(this RouteGroupBuilder group) {
        group.MapGet("/", Handle)
            .WithName("ListSharePointSites")
            .WithSummary("List SharePoint sites")
            .WithDescription("Retrieves all SharePoint sites for the specified tenant")
            .RequirePermission("Sharepoint.Site.Read", "View SharePoint sites");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string type,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetSharePointSitesQuery(tenantId, type);
            var result = await mediator.Send(query, cancellationToken);

            return Results.Ok(Response<List<SharePointSiteDto>>.SuccessResult(result, "SharePoint sites retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving SharePoint sites"
            );
        }
    }
}
