using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Api.Modules.Exchange.Queries.MessageTrace;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers.MessageTrace;

public class GetMessageTraceQueryHandler : IRequestHandler<GetMessageTraceQuery, Task<PagedResponse<MessageTraceDto>>> {
    private readonly IMessageTraceService _messageTraceService;

    public GetMessageTraceQueryHandler(IMessageTraceService messageTraceService) {
        _messageTraceService = messageTraceService;
    }

    public async Task<PagedResponse<MessageTraceDto>> Handle(GetMessageTraceQuery query, CancellationToken cancellationToken) {
        return await _messageTraceService.GetMessageTraceAsync(query.TenantId, query.SearchDto, query.PagingParams, cancellationToken);
    }
}
