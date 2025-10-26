using CIPP.Shared.DTOs.Exchange;

namespace CIPP.Api.Modules.Exchange.Interfaces;

public interface ICalendarProcessingService {
    Task<CalendarProcessingDto?> GetCalendarProcessingAsync(Guid tenantId, string mailboxIdentity, CancellationToken cancellationToken = default);
    Task UpdateCalendarProcessingAsync(Guid tenantId, string mailboxIdentity, UpdateCalendarProcessingDto updateDto, CancellationToken cancellationToken = default);
}
