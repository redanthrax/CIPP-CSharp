using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Queries;

public record GetMailboxForwardingQuery(string TenantId, string UserId) : IRequest<GetMailboxForwardingQuery, Task<MailboxForwardingDto>>;
