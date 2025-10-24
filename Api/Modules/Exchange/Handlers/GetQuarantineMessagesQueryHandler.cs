using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Api.Modules.Exchange.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers;

public class GetQuarantineMessagesQueryHandler : IRequestHandler<GetQuarantineMessagesQuery, Task<PagedResponse<QuarantineMessageDto>>> {
    private readonly ISpamFilterService _spamFilterService;

    public GetQuarantineMessagesQueryHandler(ISpamFilterService spamFilterService) {
        _spamFilterService = spamFilterService;
    }

    public async Task<PagedResponse<QuarantineMessageDto>> Handle(GetQuarantineMessagesQuery query, CancellationToken cancellationToken) {
        return await _spamFilterService.GetQuarantineMessagesAsync(query.TenantId, query.PagingParams, cancellationToken);
    }
}
