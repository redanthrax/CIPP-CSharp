using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.SharePoint.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.SharePoint;
using DispatchR;

namespace CIPP.Api.Modules.SharePoint.Endpoints.Sites;

public static class GetSharePointSettings {
    public static void MapGetSharePointSettings(this RouteGroupBuilder group) {
        group.MapGet("/settings", Handle)
            .WithName("GetSharePointSettings")
            .WithSummary("Get SharePoint settings")
            .WithDescription("Retrieves SharePoint tenant settings")
            .RequirePermission("Sharepoint.Site.Read", "View SharePoint settings");
    }

    private static async Task<IResult> Handle(
        string tenantId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetSharePointSettingsQuery(tenantId);
            var result = await mediator.Send(query, cancellationToken);

            return Results.Ok(Response<SharePointSettingsDto>.SuccessResult(result, "SharePoint settings retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving SharePoint settings"
            );
        }
    }
}
