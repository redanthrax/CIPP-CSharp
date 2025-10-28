using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Queries;

public record GetMailboxCalendarConfigurationQuery(Guid TenantId, string MailboxId)
    : IRequest<GetMailboxCalendarConfigurationQuery, Task<MailboxCalendarConfigurationDto?>>;
