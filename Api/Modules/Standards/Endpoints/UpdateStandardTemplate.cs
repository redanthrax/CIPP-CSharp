using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Standards.Commands;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Standards;
using DispatchR;

namespace CIPP.Api.Modules.Standards.Endpoints;

public static class UpdateStandardTemplate {
    public static void MapUpdateStandardTemplate(this RouteGroupBuilder group) {
        group.MapPut("/templates/{id:guid}", Handle)
            .WithName("UpdateStandardTemplate")
            .WithSummary("Update standard template")
            .WithDescription("Updates an existing standard template")
            .RequirePermission("CIPP.Standards.ReadWrite", "Update standard template");
    }

    private static async Task<IResult> Handle(
        Guid id,
        UpdateStandardTemplateDto updateDto,
        HttpContext context,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var modifiedBy = context.User.Identity?.Name ?? "system";
            var command = new UpdateStandardTemplateCommand(id, updateDto, modifiedBy);
            var result = await mediator.Send(command, cancellationToken);

            if (result == null) {
                return Results.NotFound(Response<StandardTemplateDto>.ErrorResult("Standard template not found"));
            }

            return Results.Ok(Response<StandardTemplateDto>.SuccessResult(result, "Standard template updated successfully"));
        } catch (InvalidOperationException ex) {
            return Results.BadRequest(Response<StandardTemplateDto>.ErrorResult(ex.Message));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error updating standard template"
            );
        }
    }
}
