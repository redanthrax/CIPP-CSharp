using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.ConditionalAccess.Commands;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.ConditionalAccess.Endpoints;

public static class DeleteNamedLocation {
    public static void MapDeleteNamedLocation(this RouteGroupBuilder group) {
        group.MapDelete("/named-locations/{locationId}", Handle)
            .WithName("DeleteNamedLocation")
            .WithSummary("Delete a named location")
            .WithDescription("Deletes an existing named location")
            .RequirePermission("ConditionalAccess.NamedLocation.Write", "Delete named locations");
    }

    private static async Task<IResult> Handle(
        string tenantId,
        string locationId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new DeleteNamedLocationCommand(tenantId, locationId);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<string?>.SuccessResult(null, "Named location deleted successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error deleting named location"
            );
        }
    }
}
