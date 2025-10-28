using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Api.Modules.Exchange.Queries.MobileDevices;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers.MobileDevices;

public class GetMobileDevicesQueryHandler : IRequestHandler<GetMobileDevicesQuery, Task<PagedResponse<MobileDeviceDto>>> {
    private readonly IMobileDeviceService _mobileDeviceService;

    public GetMobileDevicesQueryHandler(IMobileDeviceService mobileDeviceService) {
        _mobileDeviceService = mobileDeviceService;
    }

    public async Task<PagedResponse<MobileDeviceDto>> Handle(GetMobileDevicesQuery query, CancellationToken cancellationToken) {
        return await _mobileDeviceService.GetMobileDevicesAsync(query.TenantId, query.Mailbox, query.PagingParams, cancellationToken);
    }
}