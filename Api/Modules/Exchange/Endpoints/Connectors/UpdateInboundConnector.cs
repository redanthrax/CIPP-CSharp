using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Commands.Connectors;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.Connectors;

public static class UpdateInboundConnector {
    public static void MapUpdateInboundConnector(this RouteGroupBuilder group) {
        group.MapPut("/inbound/{connectorName}", Handle)
            .WithName("UpdateInboundConnector")
            .WithSummary("Update inbound connector")
            .WithDescription("Updates an existing inbound connector")
            .RequirePermission("Exchange.Connector.Write", "Update connectors");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string connectorName,
        UpdateInboundConnectorDto dto,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new UpdateInboundConnectorCommand(tenantId, connectorName, dto);
            await mediator.Send(command, cancellationToken);
            return Results.Ok(Response<object>.SuccessResult(null, "Inbound connector updated successfully"));
        } catch (Exception ex) {
            return Results.Problem(detail: ex.Message, statusCode: 500, title: "Error updating inbound connector");
        }
    }
}
