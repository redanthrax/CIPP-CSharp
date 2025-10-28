using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Queries.MobileDevices;

public record GetMobileDeviceQuery(Guid TenantId, string DeviceId)
    : IRequest<GetMobileDeviceQuery, Task<MobileDeviceDto?>>;