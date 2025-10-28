using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Commands.MobileDevices;

public record ClearMobileDeviceCommand(Guid TenantId, string DeviceId, ClearMobileDeviceDto ClearDto)
    : IRequest<ClearMobileDeviceCommand, Task>;