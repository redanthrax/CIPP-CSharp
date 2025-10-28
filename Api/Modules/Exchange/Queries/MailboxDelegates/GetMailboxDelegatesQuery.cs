using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Queries.MailboxDelegates;

public record GetMailboxDelegatesQuery(Guid TenantId, string MailboxId)
    : IRequest<GetMailboxDelegatesQuery, Task<List<MailboxDelegateDto>>>;
