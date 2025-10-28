using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Commands.ActiveSync;

public record CreateActiveSyncDeviceAccessRuleCommand(Guid TenantId, CreateActiveSyncDeviceAccessRuleDto CreateDto)
    : IRequest<CreateActiveSyncDeviceAccessRuleCommand, Task<string>>;