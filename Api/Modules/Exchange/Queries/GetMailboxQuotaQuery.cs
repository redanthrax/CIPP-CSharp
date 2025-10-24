using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Queries;

public record GetMailboxQuotaQuery(string TenantId, string MailboxId) : IRequest<GetMailboxQuotaQuery, Task<MailboxQuotaDto?>>;
