using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Commands;

public record UpdateInboxRuleCommand(string TenantId, string MailboxId, string RuleId, UpdateInboxRuleDto UpdateDto) : IRequest<UpdateInboxRuleCommand, Task>;
