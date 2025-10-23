using CIPP.Api.Modules.AuditLogs.Interfaces;
using CIPP.Api.Modules.AuditLogs.Queries;
using CIPP.Shared.DTOs.AuditLogs;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.AuditLogs.Handlers;

public class GetAuditLogsQueryHandler : IRequestHandler<GetAuditLogsQuery, Task<AuditLogResponseDto>> {
    private readonly IAuditLogService _auditLogService;
    private readonly ILogger<GetAuditLogsQueryHandler> _logger;

    public GetAuditLogsQueryHandler(
        IAuditLogService auditLogService,
        ILogger<GetAuditLogsQueryHandler> logger) {
        _auditLogService = auditLogService;
        _logger = logger;
    }

    public Task<AuditLogResponseDto> Handle(
        GetAuditLogsQuery request, 
        CancellationToken cancellationToken) {
        
        _logger.LogInformation(
            "Handling GetAuditLogsQuery for tenant: {TenantFilter}, logId: {LogId}",
            request.TenantFilter,
            request.LogId);

        return _auditLogService.GetAuditLogsAsync(
            request.TenantFilter,
            request.LogId,
            request.StartDate,
            request.EndDate,
            request.RelativeTime,
            cancellationToken);
    }
}