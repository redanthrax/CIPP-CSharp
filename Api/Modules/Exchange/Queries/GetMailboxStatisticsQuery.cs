using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Queries;

public record GetMailboxStatisticsQuery(string TenantId, string MailboxId) : IRequest<GetMailboxStatisticsQuery, Task<MailboxStatisticsDto?>>;
