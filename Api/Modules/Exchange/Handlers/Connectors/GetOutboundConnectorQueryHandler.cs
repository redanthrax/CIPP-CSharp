using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Api.Modules.Exchange.Queries.Connectors;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers.Connectors;

public class GetOutboundConnectorQueryHandler : IRequestHandler<GetOutboundConnectorQuery, Task<OutboundConnectorDto?>> {
    private readonly IConnectorService _connectorService;

    public GetOutboundConnectorQueryHandler(IConnectorService connectorService) {
        _connectorService = connectorService;
    }

    public async Task<OutboundConnectorDto?> Handle(GetOutboundConnectorQuery query, CancellationToken cancellationToken) {
        return await _connectorService.GetOutboundConnectorAsync(query.TenantId, query.ConnectorName);
    }
}
