using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Queries.MessageTrace;

public record GetMessageTraceDetailQuery(string TenantId, string MessageTraceId) : IRequest<GetMessageTraceDetailQuery, Task<List<MessageTraceDetailDto>>>;
