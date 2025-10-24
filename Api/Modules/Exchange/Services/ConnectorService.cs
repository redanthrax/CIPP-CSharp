using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;

namespace CIPP.Api.Modules.Exchange.Services;

public class ConnectorService : IConnectorService {
    private readonly IExchangeOnlineService _exoService;
    private readonly ILogger<ConnectorService> _logger;

    public ConnectorService(IExchangeOnlineService exoService, ILogger<ConnectorService> logger) {
        _exoService = exoService;
        _logger = logger;
    }

    public async Task<(List<InboundConnectorDto>, List<OutboundConnectorDto>)> GetConnectorsAsync(string tenantId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting connectors for tenant {TenantId}", tenantId);
        var inbound = await _exoService.ExecuteCmdletListAsync<InboundConnectorDto>(tenantId, "Get-InboundConnector", null, cancellationToken);
        var outbound = await _exoService.ExecuteCmdletListAsync<OutboundConnectorDto>(tenantId, "Get-OutboundConnector", null, cancellationToken);
        return (inbound, outbound);
    }

    public async Task<InboundConnectorDto?> GetInboundConnectorAsync(string tenantId, string connectorName, CancellationToken cancellationToken = default) {
        var parameters = new Dictionary<string, object> { { "Identity", connectorName } };
        return await _exoService.ExecuteCmdletAsync<InboundConnectorDto>(tenantId, "Get-InboundConnector", parameters, cancellationToken);
    }

    public async Task<OutboundConnectorDto?> GetOutboundConnectorAsync(string tenantId, string connectorName, CancellationToken cancellationToken = default) {
        var parameters = new Dictionary<string, object> { { "Identity", connectorName } };
        return await _exoService.ExecuteCmdletAsync<OutboundConnectorDto>(tenantId, "Get-OutboundConnector", parameters, cancellationToken);
    }

    public async Task CreateInboundConnectorAsync(string tenantId, CreateInboundConnectorDto createDto, CancellationToken cancellationToken = default) {
        var parameters = new Dictionary<string, object> {
            { "Name", createDto.Name },
            { "Enabled", createDto.Enabled },
            { "ConnectorType", createDto.ConnectorType }
        };
        if (createDto.SenderDomains?.Any() == true) parameters.Add("SenderDomains", createDto.SenderDomains.ToArray());
        if (createDto.SenderIPAddresses?.Any() == true) parameters.Add("SenderIPAddresses", createDto.SenderIPAddresses.ToArray());
        if (createDto.RequireTls.HasValue) parameters.Add("RequireTls", createDto.RequireTls.Value);
        await _exoService.ExecuteCmdletNoResultAsync(tenantId, "New-InboundConnector", parameters, cancellationToken);
    }

    public async Task CreateOutboundConnectorAsync(string tenantId, CreateOutboundConnectorDto createDto, CancellationToken cancellationToken = default) {
        var parameters = new Dictionary<string, object> {
            { "Name", createDto.Name },
            { "Enabled", createDto.Enabled },
            { "ConnectorType", createDto.ConnectorType }
        };
        if (createDto.RecipientDomains?.Any() == true) parameters.Add("RecipientDomains", createDto.RecipientDomains.ToArray());
        if (createDto.SmartHosts?.Any() == true) parameters.Add("SmartHosts", createDto.SmartHosts.ToArray());
        if (createDto.UseMXRecord.HasValue) parameters.Add("UseMXRecord", createDto.UseMXRecord.Value);
        if (createDto.RequireTls.HasValue) parameters.Add("RequireTls", createDto.RequireTls.Value);
        await _exoService.ExecuteCmdletNoResultAsync(tenantId, "New-OutboundConnector", parameters, cancellationToken);
    }

    public async Task UpdateInboundConnectorAsync(string tenantId, string connectorName, UpdateInboundConnectorDto updateDto, CancellationToken cancellationToken = default) {
        var parameters = new Dictionary<string, object> { { "Identity", connectorName } };
        if (updateDto.Enabled.HasValue) parameters.Add("Enabled", updateDto.Enabled.Value);
        if (updateDto.SenderDomains?.Any() == true) parameters.Add("SenderDomains", updateDto.SenderDomains.ToArray());
        if (updateDto.SenderIPAddresses?.Any() == true) parameters.Add("SenderIPAddresses", updateDto.SenderIPAddresses.ToArray());
        if (updateDto.RequireTls.HasValue) parameters.Add("RequireTls", updateDto.RequireTls.Value);
        await _exoService.ExecuteCmdletNoResultAsync(tenantId, "Set-InboundConnector", parameters, cancellationToken);
    }

    public async Task UpdateOutboundConnectorAsync(string tenantId, string connectorName, UpdateOutboundConnectorDto updateDto, CancellationToken cancellationToken = default) {
        var parameters = new Dictionary<string, object> { { "Identity", connectorName } };
        if (updateDto.Enabled.HasValue) parameters.Add("Enabled", updateDto.Enabled.Value);
        if (updateDto.RecipientDomains?.Any() == true) parameters.Add("RecipientDomains", updateDto.RecipientDomains.ToArray());
        if (updateDto.SmartHosts?.Any() == true) parameters.Add("SmartHosts", updateDto.SmartHosts.ToArray());
        if (updateDto.UseMXRecord.HasValue) parameters.Add("UseMXRecord", updateDto.UseMXRecord.Value);
        if (updateDto.RequireTls.HasValue) parameters.Add("RequireTls", updateDto.RequireTls.Value);
        await _exoService.ExecuteCmdletNoResultAsync(tenantId, "Set-OutboundConnector", parameters, cancellationToken);
    }
}
