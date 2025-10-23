using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.ConditionalAccess.Commands;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.ConditionalAccess;
using DispatchR;

namespace CIPP.Api.Modules.ConditionalAccess.Endpoints;

public static class CreateConditionalAccessTemplate {
    public static void MapCreateConditionalAccessTemplate(this RouteGroupBuilder group) {
        group.MapPost("/", Handle)
            .WithName("CreateConditionalAccessTemplate")
            .WithSummary("Create CA template")
            .WithDescription("Creates a new conditional access template")
            .RequirePermission("Tenant.ConditionalAccess.ReadWrite", "Create CA template");
    }

    private static async Task<IResult> Handle(
        CreateConditionalAccessTemplateDto createDto,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new CreateConditionalAccessTemplateCommand(createDto);
            var result = await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<ConditionalAccessTemplateDto>.SuccessResult(result, "Template created successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error creating template"
            );
        }
    }
}
