using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Commands.Connectors;

public record UpdateInboundConnectorCommand(string TenantId, string ConnectorName, UpdateInboundConnectorDto Connector) : IRequest<UpdateInboundConnectorCommand, Task>;
