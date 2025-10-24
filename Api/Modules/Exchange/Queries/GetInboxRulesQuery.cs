using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Queries;

public record GetInboxRulesQuery(string TenantId, string MailboxId, PagingParameters PagingParams) : IRequest<GetInboxRulesQuery, Task<PagedResponse<InboxRuleDto>>>;
