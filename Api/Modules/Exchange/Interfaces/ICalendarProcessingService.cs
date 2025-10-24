using CIPP.Shared.DTOs.Exchange;

namespace CIPP.Api.Modules.Exchange.Interfaces;

public interface ICalendarProcessingService {
    Task<CalendarProcessingDto?> GetCalendarProcessingAsync(string tenantId, string mailboxIdentity, CancellationToken cancellationToken = default);
    Task UpdateCalendarProcessingAsync(string tenantId, string mailboxIdentity, UpdateCalendarProcessingDto updateDto, CancellationToken cancellationToken = default);
}
