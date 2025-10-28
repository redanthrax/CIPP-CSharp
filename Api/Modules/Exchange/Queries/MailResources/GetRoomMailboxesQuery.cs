using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Queries.MailResources;

public record GetRoomMailboxesQuery(Guid TenantId, PagingParameters PagingParams)
    : IRequest<GetRoomMailboxesQuery, Task<PagedResponse<RoomMailboxDto>>>;
