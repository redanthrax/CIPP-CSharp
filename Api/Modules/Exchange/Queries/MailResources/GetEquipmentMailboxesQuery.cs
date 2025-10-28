using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Queries.MailResources;

public record GetEquipmentMailboxesQuery(Guid TenantId, PagingParameters PagingParams)
    : IRequest<GetEquipmentMailboxesQuery, Task<PagedResponse<EquipmentMailboxDto>>>;
