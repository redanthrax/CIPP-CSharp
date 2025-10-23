using CIPP.Api.Modules.Intune.Interfaces;
using CIPP.Api.Modules.Intune.Queries;
using CIPP.Shared.DTOs.Intune;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Intune.Handlers;

public class GetAutopilotDeviceQueryHandler : IRequestHandler<GetAutopilotDeviceQuery, Task<AutopilotDeviceDto?>> {
    private readonly IAutopilotService _autopilotService;

    public GetAutopilotDeviceQueryHandler(IAutopilotService autopilotService) {
        _autopilotService = autopilotService;
    }

    public async Task<AutopilotDeviceDto?> Handle(GetAutopilotDeviceQuery query, CancellationToken cancellationToken) {
        return await _autopilotService.GetDeviceAsync(query.TenantId, query.DeviceId, cancellationToken);
    }
}
