using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Commands.ActiveSync;

public record DeleteActiveSyncDeviceAccessRuleCommand(Guid TenantId, string RuleIdentity)
    : IRequest<DeleteActiveSyncDeviceAccessRuleCommand, Task>;