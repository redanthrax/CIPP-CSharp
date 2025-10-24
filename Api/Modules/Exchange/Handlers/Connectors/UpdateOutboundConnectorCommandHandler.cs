using CIPP.Api.Modules.Exchange.Commands.Connectors;
using CIPP.Api.Modules.Exchange.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers.Connectors;

public class UpdateOutboundConnectorCommandHandler : IRequestHandler<UpdateOutboundConnectorCommand, Task> {
    private readonly IConnectorService _connectorService;

    public UpdateOutboundConnectorCommandHandler(IConnectorService connectorService) {
        _connectorService = connectorService;
    }

    public async Task Handle(UpdateOutboundConnectorCommand command, CancellationToken cancellationToken) {
        await _connectorService.UpdateOutboundConnectorAsync(command.TenantId, command.ConnectorName, command.Connector);
    }
}
