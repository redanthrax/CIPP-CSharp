using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Api.Modules.Exchange.Queries.MobileDevices;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers.MobileDevices;

public class GetMobileDeviceStatisticsQueryHandler : IRequestHandler<GetMobileDeviceStatisticsQuery, Task<MobileDeviceStatisticsDto?>> {
    private readonly IMobileDeviceService _mobileDeviceService;

    public GetMobileDeviceStatisticsQueryHandler(IMobileDeviceService mobileDeviceService) {
        _mobileDeviceService = mobileDeviceService;
    }

    public async Task<MobileDeviceStatisticsDto?> Handle(GetMobileDeviceStatisticsQuery query, CancellationToken cancellationToken) {
        return await _mobileDeviceService.GetMobileDeviceStatisticsAsync(query.TenantId, query.DeviceId, cancellationToken);
    }
}