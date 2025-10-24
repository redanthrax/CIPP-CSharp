using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Queries.Connectors;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.Connectors;

public static class GetAllConnectors {
    public static void MapGetAllConnectors(this RouteGroupBuilder group) {
        group.MapGet("/", Handle)
            .WithName("GetAllConnectors")
            .WithSummary("Get all connectors")
            .WithDescription("Retrieves all inbound and outbound connectors for a tenant")
            .RequirePermission("Exchange.Connector.Read", "View connectors");
    }

    private static async Task<IResult> Handle(
        string tenantId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetAllConnectorsQuery(tenantId);
            var result = await mediator.Send(query, cancellationToken);

            return Results.Ok(Response<object>.SuccessResult(new { inbound = result.Inbound, outbound = result.Outbound }, "Connectors retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving connectors"
            );
        }
    }
}
