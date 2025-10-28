using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Api.Modules.Exchange.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers;

public class GetContactsQueryHandler : IRequestHandler<GetContactsQuery, Task<PagedResponse<ContactDto>>> {
    private readonly IContactService _contactService;

    public GetContactsQueryHandler(IContactService contactService) {
        _contactService = contactService;
    }

    public async Task<PagedResponse<ContactDto>> Handle(GetContactsQuery query, CancellationToken cancellationToken) {
        return await _contactService.GetContactsAsync(query.TenantId, query.PagingParams, cancellationToken);
    }
}
