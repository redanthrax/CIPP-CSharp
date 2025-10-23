using CIPP.Api.Extensions;
using CIPP.Api.Modules.Applications.Queries;
using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Applications;
using DispatchR;

namespace CIPP.Api.Modules.Applications.Endpoints;

public static class GetAppTemplates {
    public static void MapGetAppTemplates(this RouteGroupBuilder group) {
        group.MapGet("/templates", Handle)
            .WithName("GetAppTemplates")
            .WithSummary("Get all app templates")
            .WithDescription("Retrieves all application templates with pagination support")
            .RequirePermission("Applications.Template.Read", "View app templates");
    }

    private static async Task<IResult> Handle(
        HttpContext context,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var pagingParams = context.GetPagingParameters();
            var query = new GetAppTemplatesQuery(pagingParams);
            var result = await mediator.Send(query, cancellationToken);

            return Results.Ok(Response<PagedResponse<AppTemplateDto>>.SuccessResult(result, "Templates retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving app templates"
            );
        }
    }
}
