using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Api.Modules.Exchange.Queries;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers;

public class GetMailboxPermissionsQueryHandler : IRequestHandler<GetMailboxPermissionsQuery, Task<List<MailboxPermissionDto>>> {
    private readonly IMailboxService _mailboxService;

    public GetMailboxPermissionsQueryHandler(IMailboxService mailboxService) {
        _mailboxService = mailboxService;
    }

    public async Task<List<MailboxPermissionDto>> Handle(GetMailboxPermissionsQuery query, CancellationToken cancellationToken) {
        return await _mailboxService.GetMailboxPermissionsAsync(query.TenantId, query.UserId, cancellationToken);
    }
}
