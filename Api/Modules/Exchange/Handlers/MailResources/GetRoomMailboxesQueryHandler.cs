using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Api.Modules.Exchange.Queries.MailResources;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers.MailResources;

public class GetRoomMailboxesQueryHandler : IRequestHandler<GetRoomMailboxesQuery, Task<PagedResponse<RoomMailboxDto>>> {
    private readonly IMailResourceService _mailResourceService;

    public GetRoomMailboxesQueryHandler(IMailResourceService mailResourceService) {
        _mailResourceService = mailResourceService;
    }

    public async Task<PagedResponse<RoomMailboxDto>> Handle(GetRoomMailboxesQuery request, CancellationToken cancellationToken = default) {
        return await _mailResourceService.GetRoomMailboxesAsync(request.TenantId, request.PagingParams, cancellationToken);
    }
}
