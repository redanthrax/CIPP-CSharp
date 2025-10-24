using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Api.Modules.Exchange.Queries.MessageTrace;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers.MessageTrace;

public class GetMessageTraceDetailQueryHandler : IRequestHandler<GetMessageTraceDetailQuery, Task<List<MessageTraceDetailDto>>> {
    private readonly IMessageTraceService _messageTraceService;

    public GetMessageTraceDetailQueryHandler(IMessageTraceService messageTraceService) {
        _messageTraceService = messageTraceService;
    }

    public async Task<List<MessageTraceDetailDto>> Handle(GetMessageTraceDetailQuery query, CancellationToken cancellationToken) {
        return await _messageTraceService.GetMessageTraceDetailAsync(query.TenantId, query.MessageTraceId, cancellationToken);
    }
}
