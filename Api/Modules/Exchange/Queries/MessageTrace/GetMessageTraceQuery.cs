using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Queries.MessageTrace;

public record GetMessageTraceQuery(string TenantId, MessageTraceSearchDto SearchDto, PagingParameters PagingParams) : IRequest<GetMessageTraceQuery, Task<PagedResponse<MessageTraceDto>>>;
