using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Queries.JournalRules;

public record GetJournalRulesQuery(Guid TenantId, PagingParameters PagingParams)
    : IRequest<GetJournalRulesQuery, Task<PagedResponse<JournalRuleDto>>>;
