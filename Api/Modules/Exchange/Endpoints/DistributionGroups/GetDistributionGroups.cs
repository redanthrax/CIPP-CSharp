using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Queries.DistributionGroups;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.DistributionGroups;

public static class GetDistributionGroups {
    public static void MapGetDistributionGroups(this RouteGroupBuilder group) {
        group.MapGet("/", Handle)
            .WithName("GetDistributionGroups")
            .WithSummary("Get distribution groups")
            .WithDescription("Retrieves all distribution groups")
            .RequirePermission("Exchange.DistributionGroup.Read", "View distribution groups");
    }

    private static async Task<IResult> Handle(
        string tenantId,
        [AsParameters] PagingParameters pagingParams,
        string? filter,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetDistributionGroupsQuery(tenantId, pagingParams, filter);
            var result = await mediator.Send(query, cancellationToken);
            return Results.Ok(result);
        } catch (Exception ex) {
            return Results.Problem(detail: ex.Message, statusCode: 500, title: "Error retrieving distribution groups");
        }
    }
}
