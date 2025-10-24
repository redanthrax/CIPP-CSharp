using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Api.Modules.Exchange.Queries.Connectors;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers.Connectors;

public class GetAllConnectorsQueryHandler : IRequestHandler<GetAllConnectorsQuery, Task<(List<InboundConnectorDto> Inbound, List<OutboundConnectorDto> Outbound)>> {
    private readonly IConnectorService _connectorService;

    public GetAllConnectorsQueryHandler(IConnectorService connectorService) {
        _connectorService = connectorService;
    }

    public async Task<(List<InboundConnectorDto> Inbound, List<OutboundConnectorDto> Outbound)> Handle(GetAllConnectorsQuery query, CancellationToken cancellationToken) {
        return await _connectorService.GetConnectorsAsync(query.TenantId, cancellationToken);
    }
}
