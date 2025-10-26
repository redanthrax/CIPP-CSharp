using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Commands.Connectors;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.Connectors;

public static class UpdateOutboundConnector {
    public static void MapUpdateOutboundConnector(this RouteGroupBuilder group) {
        group.MapPut("/outbound/{connectorName}", Handle)
            .WithName("UpdateOutboundConnector")
            .WithSummary("Update outbound connector")
            .WithDescription("Updates an existing outbound connector")
            .RequirePermission("Exchange.Connector.Write", "Update connectors");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string connectorName,
        UpdateOutboundConnectorDto dto,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new UpdateOutboundConnectorCommand(tenantId, connectorName, dto);
            await mediator.Send(command, cancellationToken);
            return Results.Ok(Response<object>.SuccessResult(null, "Outbound connector updated successfully"));
        } catch (Exception ex) {
            return Results.Problem(detail: ex.Message, statusCode: 500, title: "Error updating outbound connector");
        }
    }
}
