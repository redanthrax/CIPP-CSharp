using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Commands.Connectors;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.Connectors;

public static class CreateOutboundConnector {
    public static void MapCreateOutboundConnector(this RouteGroupBuilder group) {
        group.MapPost("/outbound", Handle)
            .WithName("CreateOutboundConnector")
            .WithSummary("Create outbound connector")
            .WithDescription("Creates a new outbound connector")
            .RequirePermission("Exchange.Connector.Write", "Create connectors");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        CreateOutboundConnectorDto dto,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new CreateOutboundConnectorCommand(tenantId, dto);
            await mediator.Send(command, cancellationToken);
            return Results.Ok(Response<object>.SuccessResult(new { }, "Outbound connector created successfully"));
        } catch (Exception ex) {
            return Results.Problem(detail: ex.Message, statusCode: 500, title: "Error creating outbound connector");
        }
    }
}
