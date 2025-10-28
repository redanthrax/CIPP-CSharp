using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Commands.DistributionGroups;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.DistributionGroups;

public static class DeleteDistributionGroup {
    public static void MapDeleteDistributionGroup(this RouteGroupBuilder group) {
        group.MapDelete("/{groupId}", Handle)
            .WithName("DeleteDistributionGroup")
            .WithSummary("Delete distribution group")
            .WithDescription("Deletes a distribution group")
            .RequirePermission("Exchange.DistributionGroup.Write", "Delete distribution groups");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string groupId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new DeleteDistributionGroupCommand(tenantId, groupId);
            await mediator.Send(command, cancellationToken);
            return Results.Ok(Response<object>.SuccessResult(new { }, "Distribution group deleted successfully"));
        } catch (Exception ex) {
            return Results.Problem(detail: ex.Message, statusCode: 500, title: "Error deleting distribution group");
        }
    }
}
