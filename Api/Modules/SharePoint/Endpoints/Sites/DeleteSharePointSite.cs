using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.SharePoint.Commands;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.SharePoint.Endpoints.Sites;

public static class DeleteSharePointSite {
    public static void MapDeleteSharePointSite(this RouteGroupBuilder group) {
        group.MapDelete("/{siteId}", Handle)
            .WithName("DeleteSharePointSite")
            .WithSummary("Delete SharePoint site")
            .WithDescription("Deletes a SharePoint site")
            .RequirePermission("Sharepoint.Site.ReadWrite", "Delete SharePoint site");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string siteId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new DeleteSharePointSiteCommand(tenantId, siteId);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<string>.SuccessResult("Site deleted", "SharePoint site deleted successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error deleting SharePoint site"
            );
        }
    }
}
