using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Queries;

public record GetMailboxesQuery(string TenantId, string? MailboxType = null) : IRequest<GetMailboxesQuery, Task<List<MailboxDto>>>;
