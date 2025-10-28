using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Queries.MailResources;

public record GetRoomMailboxQuery(Guid TenantId, string Identity)
    : IRequest<GetRoomMailboxQuery, Task<RoomMailboxDto?>>;
