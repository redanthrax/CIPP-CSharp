using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Queries.MobileDevices;

public record GetMobileDeviceStatisticsQuery(Guid TenantId, string DeviceId)
    : IRequest<GetMobileDeviceStatisticsQuery, Task<MobileDeviceStatisticsDto?>>;