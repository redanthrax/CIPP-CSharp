using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.ConditionalAccess.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.ConditionalAccess;
using DispatchR;

namespace CIPP.Api.Modules.ConditionalAccess.Endpoints;

public static class GetConditionalAccessTemplates {
    public static void MapGetConditionalAccessTemplates(this RouteGroupBuilder group) {
        group.MapGet("/", Handle)
            .WithName("GetConditionalAccessTemplates")
            .WithSummary("Get all CA templates")
            .WithDescription("Retrieves all conditional access templates")
            .RequirePermission("Tenant.ConditionalAccess.Read", "View CA templates");
    }

    private static async Task<IResult> Handle(
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetConditionalAccessTemplatesQuery();
            var result = await mediator.Send(query, cancellationToken);

            return Results.Ok(Response<List<ConditionalAccessTemplateDto>>.SuccessResult(result, "Templates retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving templates"
            );
        }
    }
}
