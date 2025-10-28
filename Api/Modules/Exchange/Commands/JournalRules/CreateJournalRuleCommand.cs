using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Commands.JournalRules;

public record CreateJournalRuleCommand(Guid TenantId, CreateJournalRuleDto RuleData)
    : IRequest<CreateJournalRuleCommand, Task<string>>;
