using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Queries.Connectors;

public record GetOutboundConnectorQuery(string TenantId, string ConnectorName) : IRequest<GetOutboundConnectorQuery, Task<OutboundConnectorDto?>>;
