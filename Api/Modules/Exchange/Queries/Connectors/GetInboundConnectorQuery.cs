using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Queries.Connectors;

public record GetInboundConnectorQuery(string TenantId, string ConnectorName) : IRequest<GetInboundConnectorQuery, Task<InboundConnectorDto?>>;
