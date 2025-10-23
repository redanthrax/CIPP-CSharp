using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.ConditionalAccess.Commands;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.ConditionalAccess.Endpoints;

public static class DeleteConditionalAccessTemplate {
    public static void MapDeleteConditionalAccessTemplate(this RouteGroupBuilder group) {
        group.MapDelete("/{id}", Handle)
            .WithName("DeleteConditionalAccessTemplate")
            .WithSummary("Delete CA template")
            .WithDescription("Deletes a conditional access template")
            .RequirePermission("Tenant.ConditionalAccess.ReadWrite", "Delete CA template");
    }

    private static async Task<IResult> Handle(
        Guid id,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new DeleteConditionalAccessTemplateCommand(id);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<object?>.SuccessResult(null, "Template deleted successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error deleting template"
            );
        }
    }
}
