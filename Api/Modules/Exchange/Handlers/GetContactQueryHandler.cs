using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Api.Modules.Exchange.Queries;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers;

public class GetContactQueryHandler : IRequestHandler<GetContactQuery, Task<ContactDetailsDto?>> {
    private readonly IContactService _contactService;

    public GetContactQueryHandler(IContactService contactService) {
        _contactService = contactService;
    }

    public async Task<ContactDetailsDto?> Handle(GetContactQuery query, CancellationToken cancellationToken) {
        return await _contactService.GetContactAsync(query.TenantId, query.ContactId, cancellationToken);
    }
}
