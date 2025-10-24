using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Queries.CalendarProcessing;

public record GetCalendarProcessingQuery(string TenantId, string MailboxIdentity) : IRequest<GetCalendarProcessingQuery, Task<CalendarProcessingDto?>>;
