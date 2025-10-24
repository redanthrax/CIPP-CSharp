using CIPP.Api.Modules.Exchange.Commands.Connectors;
using CIPP.Api.Modules.Exchange.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers.Connectors;

public class CreateInboundConnectorCommandHandler : IRequestHandler<CreateInboundConnectorCommand, Task> {
    private readonly IConnectorService _connectorService;

    public CreateInboundConnectorCommandHandler(IConnectorService connectorService) {
        _connectorService = connectorService;
    }

    public async Task Handle(CreateInboundConnectorCommand command, CancellationToken cancellationToken) {
        await _connectorService.CreateInboundConnectorAsync(command.TenantId, command.Connector);
    }
}
