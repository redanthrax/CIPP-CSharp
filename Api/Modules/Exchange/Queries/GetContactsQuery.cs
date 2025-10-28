using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Queries;

public record GetContactsQuery(Guid TenantId, PagingParameters PagingParams)
    : IRequest<GetContactsQuery, Task<PagedResponse<ContactDto>>>;
