using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;

namespace CIPP.Api.Modules.Exchange.Interfaces;

public interface IConnectorService {
    Task<(List<InboundConnectorDto>, List<OutboundConnectorDto>)> GetConnectorsAsync(string tenantId, CancellationToken cancellationToken = default);
    Task<InboundConnectorDto?> GetInboundConnectorAsync(string tenantId, string connectorName, CancellationToken cancellationToken = default);
    Task<OutboundConnectorDto?> GetOutboundConnectorAsync(string tenantId, string connectorName, CancellationToken cancellationToken = default);
    Task CreateInboundConnectorAsync(string tenantId, CreateInboundConnectorDto createDto, CancellationToken cancellationToken = default);
    Task CreateOutboundConnectorAsync(string tenantId, CreateOutboundConnectorDto createDto, CancellationToken cancellationToken = default);
    Task UpdateInboundConnectorAsync(string tenantId, string connectorName, UpdateInboundConnectorDto updateDto, CancellationToken cancellationToken = default);
    Task UpdateOutboundConnectorAsync(string tenantId, string connectorName, UpdateOutboundConnectorDto updateDto, CancellationToken cancellationToken = default);
}

