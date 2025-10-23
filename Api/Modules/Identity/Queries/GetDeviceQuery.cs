using CIPP.Shared.DTOs.Identity;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Identity.Queries;

public record GetDeviceQuery(
    string TenantId,
    string DeviceId
) : IRequest<GetDeviceQuery, Task<DeviceDto?>>;