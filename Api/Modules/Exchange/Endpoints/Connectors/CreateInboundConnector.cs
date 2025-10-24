using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Commands.Connectors;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.Connectors;

public static class CreateInboundConnector {
    public static void MapCreateInboundConnector(this RouteGroupBuilder group) {
        group.MapPost("/inbound", Handle)
            .WithName("CreateInboundConnector")
            .WithSummary("Create inbound connector")
            .WithDescription("Creates a new inbound connector")
            .RequirePermission("Exchange.Connector.Write", "Create connectors");
    }

    private static async Task<IResult> Handle(
        string tenantId,
        CreateInboundConnectorDto dto,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new CreateInboundConnectorCommand(tenantId, dto);
            await mediator.Send(command, cancellationToken);
            return Results.Ok(Response<object>.SuccessResult(null, "Inbound connector created successfully"));
        } catch (Exception ex) {
            return Results.Problem(detail: ex.Message, statusCode: 500, title: "Error creating inbound connector");
        }
    }
}
