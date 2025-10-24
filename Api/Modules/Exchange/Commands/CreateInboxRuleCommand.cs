using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Commands;

public record CreateInboxRuleCommand(string TenantId, string MailboxId, CreateInboxRuleDto CreateDto) : IRequest<CreateInboxRuleCommand, Task<string>>;
