using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;

namespace CIPP.Api.Modules.Exchange.Interfaces;

public interface IMessageTraceService {
    Task<PagedResponse<MessageTraceDto>> GetMessageTraceAsync(Guid tenantId, MessageTraceSearchDto searchDto, PagingParameters pagingParams, CancellationToken cancellationToken = default);
    Task<List<MessageTraceDetailDto>> GetMessageTraceDetailAsync(Guid tenantId, string messageTraceId, CancellationToken cancellationToken = default);
}
