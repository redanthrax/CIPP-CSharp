using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Queries.Connectors;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.Connectors;

public static class GetOutboundConnector {
    public static void MapGetOutboundConnector(this RouteGroupBuilder group) {
        group.MapGet("/outbound/{connectorName}", Handle)
            .WithName("GetOutboundConnector")
            .WithSummary("Get outbound connector")
            .WithDescription("Retrieves a specific outbound connector")
            .RequirePermission("Exchange.Connector.Read", "View connectors");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string connectorName,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetOutboundConnectorQuery(tenantId, connectorName);
            var result = await mediator.Send(query, cancellationToken);
            
            if (result == null) {
                return Results.NotFound(Response<OutboundConnectorDto>.ErrorResult("Connector not found"));
            }

            return Results.Ok(Response<OutboundConnectorDto>.SuccessResult(result, "Outbound connector retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(detail: ex.Message, statusCode: 500, title: "Error retrieving outbound connector");
        }
    }
}
