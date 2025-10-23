using CIPP.Api.Modules.AuditLogs.Interfaces;
using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.AuditLogs;

namespace CIPP.Api.Modules.AuditLogs.Endpoints;

public static class GetAuditLogSearches {
    public static void MapGetAuditLogSearches(this RouteGroupBuilder group) {
        group.MapGet("/audit-log-searches", Handle)
            .WithName("GetAuditLogSearches")
            .WithSummary("Get audit log searches")
            .WithDescription("Returns audit log searches for a tenant with optional filtering by days. " +
                            "Parameters: tenantFilter (required), days (optional, default: 1)")
            .RequirePermission("Tenant.Alert.Read", "View audit log searches");
    }

    private static async Task<IResult> Handle(
        IAuditLogSearchService auditLogSearchService,
        string tenantFilter,
        int? days = null,
        CancellationToken cancellationToken = default) {
        
        try {
            if (string.IsNullOrEmpty(tenantFilter)) {
                return Results.BadRequest(Response<AuditLogSearchListResponseDto>.ErrorResult(
                    "TenantFilter is required",
                    new List<string> { "TenantFilter parameter is required" }
                ));
            }

            var result = await auditLogSearchService.GetSearchesAsync(tenantFilter, days, cancellationToken);

            return Results.Ok(Response<AuditLogSearchListResponseDto>.SuccessResult(result, "Audit log searches retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving audit log searches"
            );
        }
    }
}