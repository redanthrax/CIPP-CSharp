using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Commands.Connectors;

public record UpdateOutboundConnectorCommand(string TenantId, string ConnectorName, UpdateOutboundConnectorDto Connector) : IRequest<UpdateOutboundConnectorCommand, Task>;
