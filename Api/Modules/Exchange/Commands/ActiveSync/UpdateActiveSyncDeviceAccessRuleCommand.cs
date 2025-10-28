using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Commands.ActiveSync;

public record UpdateActiveSyncDeviceAccessRuleCommand(Guid TenantId, string RuleIdentity, UpdateActiveSyncDeviceAccessRuleDto UpdateDto)
    : IRequest<UpdateActiveSyncDeviceAccessRuleCommand, Task>;