using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.ConditionalAccess.Commands;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.ConditionalAccess;
using DispatchR;

namespace CIPP.Api.Modules.ConditionalAccess.Endpoints;

public static class UpdateConditionalAccessTemplate {
    public static void MapUpdateConditionalAccessTemplate(this RouteGroupBuilder group) {
        group.MapPut("/{id}", Handle)
            .WithName("UpdateConditionalAccessTemplate")
            .WithSummary("Update CA template")
            .WithDescription("Updates an existing conditional access template")
            .RequirePermission("Tenant.ConditionalAccess.ReadWrite", "Update CA template");
    }

    private static async Task<IResult> Handle(
        Guid id,
        UpdateConditionalAccessTemplateDto updateDto,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new UpdateConditionalAccessTemplateCommand(id, updateDto);
            var result = await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<ConditionalAccessTemplateDto>.SuccessResult(result, "Template updated successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error updating template"
            );
        }
    }
}
