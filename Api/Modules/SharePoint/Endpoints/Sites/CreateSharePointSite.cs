using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.SharePoint.Commands;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.SharePoint;
using DispatchR;

namespace CIPP.Api.Modules.SharePoint.Endpoints.Sites;

public static class CreateSharePointSite {
    public static void MapCreateSharePointSite(this RouteGroupBuilder group) {
        group.MapPost("/", Handle)
            .WithName("CreateSharePointSite")
            .WithSummary("Create SharePoint site")
            .WithDescription("Creates a new SharePoint site")
            .RequirePermission("Sharepoint.Site.ReadWrite", "Create SharePoint site");
    }

    private static async Task<IResult> Handle(
        string tenantId,
        CreateSharePointSiteDto request,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new CreateSharePointSiteCommand(tenantId, request);
            var result = await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<string>.SuccessResult(result, "SharePoint site created successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error creating SharePoint site"
            );
        }
    }
}
