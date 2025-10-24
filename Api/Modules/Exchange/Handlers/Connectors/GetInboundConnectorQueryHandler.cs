using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Api.Modules.Exchange.Queries.Connectors;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers.Connectors;

public class GetInboundConnectorQueryHandler : IRequestHandler<GetInboundConnectorQuery, Task<InboundConnectorDto?>> {
    private readonly IConnectorService _connectorService;

    public GetInboundConnectorQueryHandler(IConnectorService connectorService) {
        _connectorService = connectorService;
    }

    public async Task<InboundConnectorDto?> Handle(GetInboundConnectorQuery query, CancellationToken cancellationToken) {
        return await _connectorService.GetInboundConnectorAsync(query.TenantId, query.ConnectorName);
    }
}
