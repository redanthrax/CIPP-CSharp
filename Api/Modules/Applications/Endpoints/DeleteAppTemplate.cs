using CIPP.Api.Modules.Applications.Commands;
using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.Applications.Endpoints;

public static class DeleteAppTemplate {
    public static void MapDeleteAppTemplate(this RouteGroupBuilder group) {
        group.MapDelete("/templates/{id:guid}", Handle)
            .WithName("DeleteAppTemplate")
            .WithSummary("Delete an app template")
            .WithDescription("Deletes an existing application template")
            .RequirePermission("Applications.Template.Write", "Delete app templates");
    }

    private static async Task<IResult> Handle(
        Guid id,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new DeleteAppTemplateCommand(id);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<string>.SuccessResult("Template deleted successfully"));
        } catch (Exception ex) {
            return Results.Json(
                Response<string>.ErrorResult("Error deleting app template", new List<string> { ex.Message }),
                statusCode: 500
            );
        }
    }
}
