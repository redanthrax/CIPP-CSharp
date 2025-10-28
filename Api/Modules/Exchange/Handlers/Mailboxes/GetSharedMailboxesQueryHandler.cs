using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Api.Modules.Exchange.Queries.Mailboxes;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers.Mailboxes;

public class GetSharedMailboxesQueryHandler : IRequestHandler<GetSharedMailboxesQuery, Task<PagedResponse<SharedMailboxDto>>> {
    private readonly IMailboxService _mailboxService;

    public GetSharedMailboxesQueryHandler(IMailboxService mailboxService) {
        _mailboxService = mailboxService;
    }

    public async Task<PagedResponse<SharedMailboxDto>> Handle(GetSharedMailboxesQuery request, CancellationToken cancellationToken = default) {
        return await _mailboxService.GetSharedMailboxesAsync(request.TenantId, request.PagingParams, cancellationToken);
    }
}
