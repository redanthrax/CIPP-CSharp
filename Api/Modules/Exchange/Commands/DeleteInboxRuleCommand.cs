using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Commands;

public record DeleteInboxRuleCommand(string TenantId, string MailboxId, string RuleId) : IRequest<DeleteInboxRuleCommand, Task>;
