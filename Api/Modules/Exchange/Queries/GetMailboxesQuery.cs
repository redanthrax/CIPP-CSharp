using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Queries;

public record GetMailboxesQuery(Guid TenantId, string? MailboxType, PagingParameters PagingParams)
    : IRequest<GetMailboxesQuery, Task<PagedResponse<MailboxDto>>>;
