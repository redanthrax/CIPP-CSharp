using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;

namespace CIPP.Api.Modules.Exchange.Interfaces;

public interface IMessageTraceService {
    Task<PagedResponse<MessageTraceDto>> GetMessageTraceAsync(string tenantId, MessageTraceSearchDto searchDto, PagingParameters pagingParams, CancellationToken cancellationToken = default);
    Task<List<MessageTraceDetailDto>> GetMessageTraceDetailAsync(string tenantId, string messageTraceId, CancellationToken cancellationToken = default);
}
