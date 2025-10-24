using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Commands.Connectors;

public record CreateInboundConnectorCommand(string TenantId, CreateInboundConnectorDto Connector) : IRequest<CreateInboundConnectorCommand, Task>;
