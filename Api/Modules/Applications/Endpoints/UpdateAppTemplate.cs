using CIPP.Api.Modules.Applications.Commands;
using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Applications;
using DispatchR;

namespace CIPP.Api.Modules.Applications.Endpoints;

public static class UpdateAppTemplate {
    public static void MapUpdateAppTemplate(this RouteGroupBuilder group) {
        group.MapPut("/templates/{id:guid}", Handle)
            .WithName("UpdateAppTemplate")
            .WithSummary("Update an app template")
            .WithDescription("Updates an existing application template")
            .RequirePermission("Applications.Template.Write", "Update app templates");
    }

    private static async Task<IResult> Handle(
        Guid id,
        UpdateAppTemplateDto updateDto,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new UpdateAppTemplateCommand(id, updateDto);
            var result = await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<AppTemplateDto>.SuccessResult(result, "Template updated successfully"));
        } catch (Exception ex) {
            return Results.Json(
                Response<AppTemplateDto>.ErrorResult("Error updating app template", new List<string> { ex.Message }),
                statusCode: 500
            );
        }
    }
}
