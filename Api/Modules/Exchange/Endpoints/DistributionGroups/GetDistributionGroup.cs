using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Queries.DistributionGroups;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.DistributionGroups;

public static class GetDistributionGroup {
    public static void MapGetDistributionGroup(this RouteGroupBuilder group) {
        group.MapGet("/{groupId}", Handle)
            .WithName("GetDistributionGroup")
            .WithSummary("Get distribution group")
            .WithDescription("Retrieves a specific distribution group")
            .RequirePermission("Exchange.DistributionGroup.Read", "View distribution groups");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string groupId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetDistributionGroupQuery(tenantId, groupId);
            var result = await mediator.Send(query, cancellationToken);
            
            if (result == null) {
                return Results.NotFound(Response<DistributionGroupDto>.ErrorResult("Distribution group not found"));
            }

            return Results.Ok(Response<DistributionGroupDto>.SuccessResult(result, "Distribution group retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(detail: ex.Message, statusCode: 500, title: "Error retrieving distribution group");
        }
    }
}
