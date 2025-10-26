using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Queries.DistributionGroups;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.DistributionGroups;

public static class GetDistributionGroupMembers {
    public static void MapGetDistributionGroupMembers(this RouteGroupBuilder group) {
        group.MapGet("/{groupId}/members", Handle)
            .WithName("GetDistributionGroupMembers")
            .WithSummary("Get group members")
            .WithDescription("Retrieves members of a distribution group")
            .RequirePermission("Exchange.DistributionGroup.Read", "View distribution groups");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string groupId,
        [AsParameters] PagingParameters pagingParams,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetDistributionGroupMembersQuery(tenantId, groupId, pagingParams);
            var result = await mediator.Send(query, cancellationToken);
            return Results.Ok(result);
        } catch (Exception ex) {
            return Results.Problem(detail: ex.Message, statusCode: 500, title: "Error retrieving group members");
        }
    }
}
