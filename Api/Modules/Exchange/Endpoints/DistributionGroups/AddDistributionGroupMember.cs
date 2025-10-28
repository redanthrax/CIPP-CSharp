using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Commands.DistributionGroups;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.DistributionGroups;

public static class AddDistributionGroupMember {
    public static void MapAddDistributionGroupMember(this RouteGroupBuilder group) {
        group.MapPost("/{groupId}/members", Handle)
            .WithName("AddDistributionGroupMember")
            .WithSummary("Add group member")
            .WithDescription("Adds a member to a distribution group")
            .RequirePermission("Exchange.DistributionGroup.Write", "Update distribution groups");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string groupId,
        AddMemberRequest request,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new AddDistributionGroupMemberCommand(tenantId, groupId, request.MemberEmail);
            await mediator.Send(command, cancellationToken);
            return Results.Ok(Response<object>.SuccessResult(new { }, "Member added successfully"));
        } catch (Exception ex) {
            return Results.Problem(detail: ex.Message, statusCode: 500, title: "Error adding group member");
        }
    }
}

public record AddMemberRequest(string MemberEmail);
