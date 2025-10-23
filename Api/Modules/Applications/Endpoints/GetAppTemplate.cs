using CIPP.Api.Modules.Applications.Queries;
using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Applications;
using DispatchR;

namespace CIPP.Api.Modules.Applications.Endpoints;

public static class GetAppTemplate {
    public static void MapGetAppTemplate(this RouteGroupBuilder group) {
        group.MapGet("/templates/{id:guid}", Handle)
            .WithName("GetAppTemplate")
            .WithSummary("Get an app template")
            .WithDescription("Retrieves a specific application template by ID")
            .RequirePermission("Applications.Template.Read", "View app templates");
    }

    private static async Task<IResult> Handle(
        Guid id,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetAppTemplateQuery(id);
            var result = await mediator.Send(query, cancellationToken);

            if (result == null) {
                return Results.NotFound(Response<AppTemplateDto>.ErrorResult("Template not found"));
            }

            return Results.Ok(Response<AppTemplateDto>.SuccessResult(result, "Template retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving app template"
            );
        }
    }
}
