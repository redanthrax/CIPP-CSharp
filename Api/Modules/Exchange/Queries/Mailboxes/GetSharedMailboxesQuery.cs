using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Queries.Mailboxes;

public record GetSharedMailboxesQuery(Guid TenantId, PagingParameters PagingParams)
    : IRequest<GetSharedMailboxesQuery, Task<PagedResponse<SharedMailboxDto>>>;
