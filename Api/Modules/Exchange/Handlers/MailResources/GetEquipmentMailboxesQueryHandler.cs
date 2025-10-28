using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Api.Modules.Exchange.Queries.MailResources;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers.MailResources;

public class GetEquipmentMailboxesQueryHandler : IRequestHandler<GetEquipmentMailboxesQuery, Task<PagedResponse<EquipmentMailboxDto>>> {
    private readonly IMailResourceService _mailResourceService;

    public GetEquipmentMailboxesQueryHandler(IMailResourceService mailResourceService) {
        _mailResourceService = mailResourceService;
    }

    public async Task<PagedResponse<EquipmentMailboxDto>> Handle(GetEquipmentMailboxesQuery request, CancellationToken cancellationToken = default) {
        return await _mailResourceService.GetEquipmentMailboxesAsync(request.TenantId, request.PagingParams, cancellationToken);
    }
}
