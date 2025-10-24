using CIPP.Api.Modules.Exchange.Commands.Connectors;
using CIPP.Api.Modules.Exchange.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers.Connectors;

public class CreateOutboundConnectorCommandHandler : IRequestHandler<CreateOutboundConnectorCommand, Task> {
    private readonly IConnectorService _connectorService;

    public CreateOutboundConnectorCommandHandler(IConnectorService connectorService) {
        _connectorService = connectorService;
    }

    public async Task Handle(CreateOutboundConnectorCommand command, CancellationToken cancellationToken) {
        await _connectorService.CreateOutboundConnectorAsync(command.TenantId, command.Connector);
    }
}
