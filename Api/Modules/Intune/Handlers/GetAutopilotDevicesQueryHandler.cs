using CIPP.Api.Modules.Intune.Interfaces;
using CIPP.Api.Modules.Intune.Queries;
using CIPP.Shared.DTOs.Intune;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Intune.Handlers;

public class GetAutopilotDevicesQueryHandler : IRequestHandler<GetAutopilotDevicesQuery, Task<List<AutopilotDeviceDto>>> {
    private readonly IAutopilotService _autopilotService;

    public GetAutopilotDevicesQueryHandler(IAutopilotService autopilotService) {
        _autopilotService = autopilotService;
    }

    public async Task<List<AutopilotDeviceDto>> Handle(GetAutopilotDevicesQuery query, CancellationToken cancellationToken) {
        return await _autopilotService.GetDevicesAsync(query.TenantId, cancellationToken);
    }
}
