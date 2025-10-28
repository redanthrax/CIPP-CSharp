using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Commands.JournalRules;

public record DeleteJournalRuleCommand(Guid TenantId, string RuleName)
    : IRequest<DeleteJournalRuleCommand, Task>;
