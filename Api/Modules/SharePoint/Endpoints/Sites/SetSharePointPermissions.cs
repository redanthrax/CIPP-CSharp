using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.SharePoint.Commands;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.SharePoint;
using DispatchR;

namespace CIPP.Api.Modules.SharePoint.Endpoints.Sites;

public static class SetSharePointPermissions {
    public static void MapSetSharePointPermissions(this RouteGroupBuilder group) {
        group.MapPost("/permissions", Handle)
            .WithName("SetSharePointPermissions")
            .WithSummary("Set SharePoint permissions")
            .WithDescription("Sets SharePoint site or OneDrive permissions")
            .RequirePermission("Sharepoint.Site.ReadWrite", "Manage SharePoint permissions");
    }

    private static async Task<IResult> Handle(
        string tenantId,
        SetSharePointPermissionsDto request,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new SetSharePointPermissionsCommand(
                tenantId,
                request.UserId,
                request.AccessUser,
                request.Url,
                request.RemovePermission
            );
            var result = await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<string>.SuccessResult(result, "Permissions updated successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error setting SharePoint permissions"
            );
        }
    }
}
