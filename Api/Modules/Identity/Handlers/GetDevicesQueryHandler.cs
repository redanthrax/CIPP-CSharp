using CIPP.Api.Modules.Identity.Interfaces;
using CIPP.Api.Modules.Identity.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Identity;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Identity.Handlers;

public class GetDevicesQueryHandler : IRequestHandler<GetDevicesQuery, Task<PagedResponse<DeviceDto>>> {
    private readonly IDeviceService _deviceService;
    private readonly ILogger<GetDevicesQueryHandler> _logger;

    public GetDevicesQueryHandler(
        IDeviceService deviceService,
        ILogger<GetDevicesQueryHandler> logger) {
        _deviceService = deviceService;
        _logger = logger;
    }

    public async Task<PagedResponse<DeviceDto>> Handle(GetDevicesQuery request, CancellationToken cancellationToken) {
        try {
            _logger.LogInformation("Getting devices for tenant {TenantId}", request.TenantId);
            
            var result = await _deviceService.GetDevicesAsync(request.TenantId, request.Paging, cancellationToken);
            return result;
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to get devices for tenant {TenantId}", request.TenantId);
            return new PagedResponse<DeviceDto> {
                Items = new List<DeviceDto>(),
                TotalCount = 0,
                PageNumber = request.Paging?.PageNumber ?? 1,
                PageSize = request.Paging?.PageSize ?? 50
            };
        }
    }
}
