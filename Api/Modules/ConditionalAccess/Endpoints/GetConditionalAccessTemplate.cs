using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.ConditionalAccess.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.ConditionalAccess;
using DispatchR;

namespace CIPP.Api.Modules.ConditionalAccess.Endpoints;

public static class GetConditionalAccessTemplate {
    public static void MapGetConditionalAccessTemplate(this RouteGroupBuilder group) {
        group.MapGet("/{id}", Handle)
            .WithName("GetConditionalAccessTemplate")
            .WithSummary("Get CA template by ID")
            .WithDescription("Retrieves a specific conditional access template by ID")
            .RequirePermission("Tenant.ConditionalAccess.Read", "View CA template");
    }

    private static async Task<IResult> Handle(
        Guid id,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetConditionalAccessTemplateQuery(id);
            var result = await mediator.Send(query, cancellationToken);

            if (result == null) {
                return Results.NotFound($"Template with ID {id} not found");
            }

            return Results.Ok(Response<ConditionalAccessTemplateDto>.SuccessResult(result, "Template retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving template"
            );
        }
    }
}
