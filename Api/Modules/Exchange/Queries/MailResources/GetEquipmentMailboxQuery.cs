using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Queries.MailResources;

public record GetEquipmentMailboxQuery(Guid TenantId, string Identity)
    : IRequest<GetEquipmentMailboxQuery, Task<EquipmentMailboxDto?>>;
