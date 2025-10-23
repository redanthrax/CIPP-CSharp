using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.SharePoint.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.SharePoint;
using DispatchR;

namespace CIPP.Api.Modules.SharePoint.Endpoints.Sites;

public static class GetSharePointQuota {
    public static void MapGetSharePointQuota(this RouteGroupBuilder group) {
        group.MapGet("/quota", Handle)
            .WithName("GetSharePointQuota")
            .WithSummary("Get SharePoint quota")
            .WithDescription("Retrieves SharePoint tenant storage quota")
            .RequirePermission("Sharepoint.Site.Read", "View SharePoint quota");
    }

    private static async Task<IResult> Handle(
        string tenantId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetSharePointQuotaQuery(tenantId);
            var result = await mediator.Send(query, cancellationToken);

            return Results.Ok(Response<SharePointQuotaDto>.SuccessResult(result, "SharePoint quota retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving SharePoint quota"
            );
        }
    }
}
