using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Api.Modules.Exchange.Queries.MobileDevices;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers.MobileDevices;

public class GetMobileDeviceQueryHandler : IRequestHandler<GetMobileDeviceQuery, Task<MobileDeviceDto?>> {
    private readonly IMobileDeviceService _mobileDeviceService;

    public GetMobileDeviceQueryHandler(IMobileDeviceService mobileDeviceService) {
        _mobileDeviceService = mobileDeviceService;
    }

    public async Task<MobileDeviceDto?> Handle(GetMobileDeviceQuery query, CancellationToken cancellationToken) {
        return await _mobileDeviceService.GetMobileDeviceAsync(query.TenantId, query.DeviceId, cancellationToken);
    }
}