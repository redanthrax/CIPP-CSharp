using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Commands.DistributionGroups;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.DistributionGroups;

public static class UpdateDistributionGroup {
    public static void MapUpdateDistributionGroup(this RouteGroupBuilder group) {
        group.MapPut("/{groupId}", Handle)
            .WithName("UpdateDistributionGroup")
            .WithSummary("Update distribution group")
            .WithDescription("Updates an existing distribution group")
            .RequirePermission("Exchange.DistributionGroup.Write", "Update distribution groups");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string groupId,
        UpdateDistributionGroupDto dto,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new UpdateDistributionGroupCommand(tenantId, groupId, dto);
            await mediator.Send(command, cancellationToken);
            return Results.Ok(Response<object>.SuccessResult(null, "Distribution group updated successfully"));
        } catch (Exception ex) {
            return Results.Problem(detail: ex.Message, statusCode: 500, title: "Error updating distribution group");
        }
    }
}
