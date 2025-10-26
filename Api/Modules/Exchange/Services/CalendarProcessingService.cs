using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Shared.DTOs.Exchange;
using System.Reflection;

namespace CIPP.Api.Modules.Exchange.Services;

public class CalendarProcessingService : ICalendarProcessingService {
    private readonly IExchangeOnlineService _exoService;
    private readonly ILogger<CalendarProcessingService> _logger;

    public CalendarProcessingService(IExchangeOnlineService exoService, ILogger<CalendarProcessingService> logger) {
        _exoService = exoService;
        _logger = logger;
    }

    public async Task<CalendarProcessingDto?> GetCalendarProcessingAsync(Guid tenantId, string mailboxIdentity, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting calendar processing settings for mailbox {MailboxIdentity} in tenant {TenantId}", mailboxIdentity, tenantId);
        var parameters = new Dictionary<string, object> { { "Identity", mailboxIdentity } };
        return await _exoService.ExecuteCmdletAsync<CalendarProcessingDto>(tenantId, "Get-CalendarProcessing", parameters, cancellationToken);
    }

    public async Task UpdateCalendarProcessingAsync(Guid tenantId, string mailboxIdentity, UpdateCalendarProcessingDto updateDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Updating calendar processing settings for mailbox {MailboxIdentity} in tenant {TenantId}", mailboxIdentity, tenantId);
        
        var parameters = new Dictionary<string, object> { { "Identity", mailboxIdentity } };
        
        foreach (var property in typeof(UpdateCalendarProcessingDto).GetProperties(BindingFlags.Public | BindingFlags.Instance)) {
            var value = property.GetValue(updateDto);
            
            if (value == null) continue;
            
            if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) {
                parameters.Add(property.Name, value);
            }
            else if (property.PropertyType == typeof(List<string>) && value is List<string> list && list.Any()) {
                parameters.Add(property.Name, list.ToArray());
            }
            else if (value is string str && !string.IsNullOrEmpty(str)) {
                parameters.Add(property.Name, str);
            }
        }

        await _exoService.ExecuteCmdletNoResultAsync(tenantId, "Set-CalendarProcessing", parameters, cancellationToken);
    }
}
