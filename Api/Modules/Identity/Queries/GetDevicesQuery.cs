using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Identity;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Identity.Queries;

public record GetDevicesQuery(
    string TenantId,
    PagingParameters? Paging = null
) : IRequest<GetDevicesQuery, Task<PagedResponse<DeviceDto>>>;
