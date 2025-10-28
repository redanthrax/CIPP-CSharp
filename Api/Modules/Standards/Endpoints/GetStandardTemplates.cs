using CIPP.Api.Extensions;
using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Standards.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Standards;
using DispatchR;

namespace CIPP.Api.Modules.Standards.Endpoints;

public static class GetStandardTemplates {
    public static void MapGetStandardTemplates(this RouteGroupBuilder group) {
        group.MapGet("/templates", Handle)
            .WithName("GetStandardTemplates")
            .WithSummary("Get standard templates")
            .WithDescription("Returns a paginated list of standard templates")
            .RequirePermission("CIPP.Standards.Read", "View standard templates");
    }

    private static async Task<IResult> Handle(
        string? type,
        string? category,
        HttpContext context,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var pagingParams = context.GetPagingParameters();
            var query = new GetStandardTemplatesQuery(type, category, pagingParams);
            var result = await mediator.Send(query, cancellationToken);

            return Results.Ok(Response<PagedResponse<StandardTemplateDto>>.SuccessResult(result, "Standard templates retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving standard templates"
            );
        }
    }
}
