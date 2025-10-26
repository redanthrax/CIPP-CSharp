using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Queries.Connectors;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.Connectors;

public static class GetInboundConnector {
    public static void MapGetInboundConnector(this RouteGroupBuilder group) {
        group.MapGet("/inbound/{connectorName}", Handle)
            .WithName("GetInboundConnector")
            .WithSummary("Get inbound connector")
            .WithDescription("Retrieves a specific inbound connector")
            .RequirePermission("Exchange.Connector.Read", "View connectors");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string connectorName,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetInboundConnectorQuery(tenantId, connectorName);
            var result = await mediator.Send(query, cancellationToken);
            
            if (result == null) {
                return Results.NotFound(Response<InboundConnectorDto>.ErrorResult("Connector not found"));
            }

            return Results.Ok(Response<InboundConnectorDto>.SuccessResult(result, "Inbound connector retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(detail: ex.Message, statusCode: 500, title: "Error retrieving inbound connector");
        }
    }
}
