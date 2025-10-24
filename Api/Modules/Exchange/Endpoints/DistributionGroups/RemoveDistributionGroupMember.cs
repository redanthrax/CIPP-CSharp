using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Commands.DistributionGroups;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.DistributionGroups;

public static class RemoveDistributionGroupMember {
    public static void MapRemoveDistributionGroupMember(this RouteGroupBuilder group) {
        group.MapDelete("/{groupId}/members/{memberEmail}", Handle)
            .WithName("RemoveDistributionGroupMember")
            .WithSummary("Remove group member")
            .WithDescription("Removes a member from a distribution group")
            .RequirePermission("Exchange.DistributionGroup.Write", "Update distribution groups");
    }

    private static async Task<IResult> Handle(
        string tenantId,
        string groupId,
        string memberEmail,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new RemoveDistributionGroupMemberCommand(tenantId, groupId, memberEmail);
            await mediator.Send(command, cancellationToken);
            return Results.Ok(Response<object>.SuccessResult(null, "Member removed successfully"));
        } catch (Exception ex) {
            return Results.Problem(detail: ex.Message, statusCode: 500, title: "Error removing group member");
        }
    }
}
