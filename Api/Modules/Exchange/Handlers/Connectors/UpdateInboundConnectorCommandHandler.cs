using CIPP.Api.Modules.Exchange.Commands.Connectors;
using CIPP.Api.Modules.Exchange.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers.Connectors;

public class UpdateInboundConnectorCommandHandler : IRequestHandler<UpdateInboundConnectorCommand, Task> {
    private readonly IConnectorService _connectorService;

    public UpdateInboundConnectorCommandHandler(IConnectorService connectorService) {
        _connectorService = connectorService;
    }

    public async Task Handle(UpdateInboundConnectorCommand command, CancellationToken cancellationToken) {
        await _connectorService.UpdateInboundConnectorAsync(command.TenantId, command.ConnectorName, command.Connector);
    }
}
