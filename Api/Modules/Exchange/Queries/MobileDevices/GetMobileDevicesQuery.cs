using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Queries.MobileDevices;

public record GetMobileDevicesQuery(Guid TenantId, string? Mailbox, PagingParameters PagingParams)
    : IRequest<GetMobileDevicesQuery, Task<PagedResponse<MobileDeviceDto>>>;
