using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Queries;

public record GetMailboxAutoReplyConfigurationQuery(Guid TenantId, string MailboxId)
    : IRequest<GetMailboxAutoReplyConfigurationQuery, Task<MailboxAutoReplyConfigurationDto?>>;
