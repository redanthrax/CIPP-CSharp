using CIPP.Api.Modules.Applications.Commands;
using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Applications;
using DispatchR;

namespace CIPP.Api.Modules.Applications.Endpoints;

public static class CreateAppTemplate {
    public static void MapCreateAppTemplate(this RouteGroupBuilder group) {
        group.MapPost("/templates", Handle)
            .WithName("CreateAppTemplate")
            .WithSummary("Create an app template")
            .WithDescription("Creates a new application template")
            .RequirePermission("Applications.Template.Write", "Create app templates");
    }

    private static async Task<IResult> Handle(
        CreateAppTemplateDto createDto,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new CreateAppTemplateCommand(createDto);
            var result = await mediator.Send(command, cancellationToken);

            return Results.Created($"/api/applications/templates/{result.Id}", Response<AppTemplateDto>.SuccessResult(result, "Template created successfully"));
        } catch (Exception ex) {
            return Results.Json(
                Response<AppTemplateDto>.ErrorResult("Error creating app template", new List<string> { ex.Message }),
                statusCode: 500
            );
        }
    }
}
