using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Standards.Commands;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Standards;
using DispatchR;

namespace CIPP.Api.Modules.Standards.Endpoints;

public static class CreateStandardTemplate {
    public static void MapCreateStandardTemplate(this RouteGroupBuilder group) {
        group.MapPost("/templates", Handle)
            .WithName("CreateStandardTemplate")
            .WithSummary("Create standard template")
            .WithDescription("Creates a new standard template")
            .RequirePermission("CIPP.Standards.ReadWrite", "Create standard template");
    }

    private static async Task<IResult> Handle(
        CreateStandardTemplateDto createDto,
        HttpContext context,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var createdBy = context.User.Identity?.Name ?? "system";
            var command = new CreateStandardTemplateCommand(createDto, createdBy);
            var result = await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<StandardTemplateDto>.SuccessResult(result, "Standard template created successfully"));
        } catch (InvalidOperationException ex) {
            return Results.BadRequest(Response<StandardTemplateDto>.ErrorResult(ex.Message));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error creating standard template"
            );
        }
    }
}
