using CIPP.Shared.DTOs.Intune;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Intune.Queries;

public record GetAutopilotDevicesQuery(string TenantId) 
    : IRequest<GetAutopilotDevicesQuery, Task<List<AutopilotDeviceDto>>>;
