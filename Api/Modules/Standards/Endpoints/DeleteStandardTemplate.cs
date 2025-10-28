using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Standards.Commands;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.Standards.Endpoints;

public static class DeleteStandardTemplate {
    public static void MapDeleteStandardTemplate(this RouteGroupBuilder group) {
        group.MapDelete("/templates/{id:guid}", Handle)
            .WithName("DeleteStandardTemplate")
            .WithSummary("Delete standard template")
            .WithDescription("Deletes a standard template")
            .RequirePermission("CIPP.Standards.ReadWrite", "Delete standard template");
    }

    private static async Task<IResult> Handle(
        Guid id,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new DeleteStandardTemplateCommand(id);
            var result = await mediator.Send(command, cancellationToken);

            if (!result) {
                return Results.NotFound(Response<bool>.ErrorResult("Standard template not found"));
            }

            return Results.Ok(Response<bool>.SuccessResult(result, "Standard template deleted successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error deleting standard template"
            );
        }
    }
}
