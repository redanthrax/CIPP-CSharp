using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Api.Modules.Exchange.Queries.MailResources;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers.MailResources;

public class GetRoomMailboxQueryHandler : IRequestHandler<GetRoomMailboxQuery, Task<RoomMailboxDto?>> {
    private readonly IMailResourceService _mailResourceService;

    public GetRoomMailboxQueryHandler(IMailResourceService mailResourceService) {
        _mailResourceService = mailResourceService;
    }

    public async Task<RoomMailboxDto?> Handle(GetRoomMailboxQuery request, CancellationToken cancellationToken = default) {
        return await _mailResourceService.GetRoomMailboxAsync(request.TenantId, request.Identity, cancellationToken);
    }
}
