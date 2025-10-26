using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;

namespace CIPP.Api.Modules.Exchange.Interfaces;

public interface IConnectorService {
    Task<(List<InboundConnectorDto>, List<OutboundConnectorDto>)> GetConnectorsAsync(Guid tenantId, CancellationToken cancellationToken = default);
    Task<InboundConnectorDto?> GetInboundConnectorAsync(Guid tenantId, string connectorName, CancellationToken cancellationToken = default);
    Task<OutboundConnectorDto?> GetOutboundConnectorAsync(Guid tenantId, string connectorName, CancellationToken cancellationToken = default);
    Task CreateInboundConnectorAsync(Guid tenantId, CreateInboundConnectorDto createDto, CancellationToken cancellationToken = default);
    Task CreateOutboundConnectorAsync(Guid tenantId, CreateOutboundConnectorDto createDto, CancellationToken cancellationToken = default);
    Task UpdateInboundConnectorAsync(Guid tenantId, string connectorName, UpdateInboundConnectorDto updateDto, CancellationToken cancellationToken = default);
    Task UpdateOutboundConnectorAsync(Guid tenantId, string connectorName, UpdateOutboundConnectorDto updateDto, CancellationToken cancellationToken = default);
}

