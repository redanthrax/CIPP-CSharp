using CIPP.Api.Modules.AuditLogs.Queries;
using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.AuditLogs;
using DispatchR;

namespace CIPP.Api.Modules.AuditLogs.Endpoints;

public static class GetAuditLogs {
    public static void MapGetAuditLogs(this RouteGroupBuilder group) {
        group.MapGet("/audit-logs", Handle)
            .WithName("GetAuditLogs")
            .WithSummary("Get audit logs")
            .WithDescription("Returns audit logs with optional filtering by tenant, date range, or specific log ID. " +
                            "Parameters: tenantFilter, logId, startDate, endDate, relativeTime (e.g., '7d', '24h', '30m')")
            .RequirePermission("CIPP.Alert.Read", "View audit logs");
    }

    private static async Task<IResult> Handle(
        IMediator mediator,
        string? tenantFilter = null,
        string? logId = null,
        string? startDate = null,
        string? endDate = null,
        string? relativeTime = null,
        CancellationToken cancellationToken = default) {
        
        try {
            var query = new GetAuditLogsQuery(
                TenantFilter: tenantFilter,
                LogId: logId,
                StartDate: startDate,
                EndDate: endDate,
                RelativeTime: relativeTime
            );

            var auditLogResponse = await mediator.Send(query, cancellationToken);

            return Results.Ok(Response<AuditLogResponseDto>.SuccessResult(auditLogResponse, "Audit logs retrieved successfully"));
        } catch (ArgumentException ex) {
            return Results.BadRequest(Response<AuditLogResponseDto>.ErrorResult(
                ex.Message, 
                new List<string> { "Invalid parameter format" }
            ));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving audit logs"
            );
        }
    }
}