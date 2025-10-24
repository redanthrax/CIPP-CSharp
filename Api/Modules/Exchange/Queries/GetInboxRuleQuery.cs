using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Queries;

public record GetInboxRuleQuery(string TenantId, string MailboxId, string RuleId) : IRequest<GetInboxRuleQuery, Task<InboxRuleDto?>>;
