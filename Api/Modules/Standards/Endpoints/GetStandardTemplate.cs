using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Standards.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Standards;
using DispatchR;

namespace CIPP.Api.Modules.Standards.Endpoints;

public static class GetStandardTemplate {
    public static void MapGetStandardTemplate(this RouteGroupBuilder group) {
        group.MapGet("/templates/{id:guid}", Handle)
            .WithName("GetStandardTemplate")
            .WithSummary("Get standard template")
            .WithDescription("Returns a specific standard template by ID")
            .RequirePermission("CIPP.Standards.Read", "View standard template");
    }

    private static async Task<IResult> Handle(
        Guid id,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetStandardTemplateQuery(id);
            var result = await mediator.Send(query, cancellationToken);

            if (result == null) {
                return Results.NotFound(Response<StandardTemplateDto>.ErrorResult("Standard template not found"));
            }

            return Results.Ok(Response<StandardTemplateDto>.SuccessResult(result, "Standard template retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving standard template"
            );
        }
    }
}
