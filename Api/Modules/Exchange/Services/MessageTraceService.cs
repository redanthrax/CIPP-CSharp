using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using System.Reflection;

namespace CIPP.Api.Modules.Exchange.Services;

public class MessageTraceService : IMessageTraceService {
    private readonly IExchangeOnlineService _exoService;
    private readonly ILogger<MessageTraceService> _logger;

    public MessageTraceService(IExchangeOnlineService exoService, ILogger<MessageTraceService> logger) {
        _exoService = exoService;
        _logger = logger;
    }

    public async Task<PagedResponse<MessageTraceDto>> GetMessageTraceAsync(Guid tenantId, MessageTraceSearchDto searchDto, PagingParameters pagingParams, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting message trace for tenant {TenantId}", tenantId);
        
        var parameters = new Dictionary<string, object>();
        
        foreach (var property in typeof(MessageTraceSearchDto).GetProperties(BindingFlags.Public | BindingFlags.Instance)) {
            var value = property.GetValue(searchDto);
            if (value == null) continue;
            
            if (property.Name == "PageSize" || property.Name == "Page") continue;
            
            if (value is DateTime dateValue) {
                parameters.Add(property.Name, dateValue);
            }
            else if (value is string str && !string.IsNullOrEmpty(str)) {
                parameters.Add(property.Name, str);
            }
        }
        
        if (pagingParams.PageSize > 0) {
            parameters.Add("PageSize", Math.Min(pagingParams.PageSize, 5000));
        }
        if (pagingParams.PageNumber > 1) {
            parameters.Add("Page", pagingParams.PageNumber);
        }

        var messages = await _exoService.ExecuteCmdletListAsync<MessageTraceDto>(tenantId, "Get-MessageTrace", parameters, cancellationToken);
        
        return new PagedResponse<MessageTraceDto> {
            Items = messages,
            TotalCount = messages.Count,
            PageNumber = pagingParams.PageNumber,
            PageSize = pagingParams.PageSize
        };
    }

    public async Task<List<MessageTraceDetailDto>> GetMessageTraceDetailAsync(Guid tenantId, string messageTraceId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting message trace detail for message {MessageTraceId} in tenant {TenantId}", messageTraceId, tenantId);
        var parameters = new Dictionary<string, object> { { "MessageTraceId", messageTraceId } };
        return await _exoService.ExecuteCmdletListAsync<MessageTraceDetailDto>(tenantId, "Get-MessageTraceDetail", parameters, cancellationToken);
    }
}
