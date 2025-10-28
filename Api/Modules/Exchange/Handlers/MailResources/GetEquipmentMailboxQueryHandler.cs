using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Api.Modules.Exchange.Queries.MailResources;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers.MailResources;

public class GetEquipmentMailboxQueryHandler : IRequestHandler<GetEquipmentMailboxQuery, Task<EquipmentMailboxDto?>> {
    private readonly IMailResourceService _mailResourceService;

    public GetEquipmentMailboxQueryHandler(IMailResourceService mailResourceService) {
        _mailResourceService = mailResourceService;
    }

    public async Task<EquipmentMailboxDto?> Handle(GetEquipmentMailboxQuery request, CancellationToken cancellationToken = default) {
        return await _mailResourceService.GetEquipmentMailboxAsync(request.TenantId, request.Identity, cancellationToken);
    }
}
