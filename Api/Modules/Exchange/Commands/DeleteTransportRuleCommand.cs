using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Commands;

public record DeleteTransportRuleCommand(string TenantId, string RuleId) : IRequest<DeleteTransportRuleCommand, Task>;
