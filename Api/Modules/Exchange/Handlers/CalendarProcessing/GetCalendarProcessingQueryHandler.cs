using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Api.Modules.Exchange.Queries.CalendarProcessing;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers.CalendarProcessing;

public class GetCalendarProcessingQueryHandler : IRequestHandler<GetCalendarProcessingQuery, Task<CalendarProcessingDto?>> {
    private readonly ICalendarProcessingService _calendarProcessingService;

    public GetCalendarProcessingQueryHandler(ICalendarProcessingService calendarProcessingService) {
        _calendarProcessingService = calendarProcessingService;
    }

    public async Task<CalendarProcessingDto?> Handle(GetCalendarProcessingQuery query, CancellationToken cancellationToken) {
        return await _calendarProcessingService.GetCalendarProcessingAsync(query.TenantId, query.MailboxIdentity, cancellationToken);
    }
}
