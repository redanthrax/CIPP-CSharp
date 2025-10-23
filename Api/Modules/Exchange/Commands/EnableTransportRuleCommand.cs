using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Commands;

public record EnableTransportRuleCommand(string TenantId, string RuleId, bool Enable) : IRequest<EnableTransportRuleCommand, Task>;
