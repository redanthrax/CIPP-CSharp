using CIPP.Api.Modules.AuditLogs.Interfaces;
using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.AuditLogs;

namespace CIPP.Api.Modules.AuditLogs.Endpoints;

public static class ProcessAuditLogSearch {
    public static void MapProcessAuditLogSearch(this RouteGroupBuilder group) {
        group.MapPost("/audit-log-searches/process", Handle)
            .WithName("ProcessAuditLogSearch")
            .WithSummary("Process audit log search")
            .WithDescription("Processes an existing audit log search to analyze results. " +
                            "Parameters: tenantFilter (required), searchId (required)")
            .RequirePermission("Tenant.Alert.ReadWrite", "Process audit log searches");
    }

    private static async Task<IResult> Handle(
        IAuditLogSearchService auditLogSearchService,
        string tenantFilter,
        string searchId,
        CancellationToken cancellationToken = default) {
        
        try {
            if (string.IsNullOrEmpty(tenantFilter)) {
                return Results.BadRequest(Response<AuditLogSearchDto>.ErrorResult(
                    "TenantFilter is required",
                    new List<string> { "TenantFilter parameter is required" }
                ));
            }

            if (string.IsNullOrEmpty(searchId)) {
                return Results.BadRequest(Response<AuditLogSearchDto>.ErrorResult(
                    "SearchId is required",
                    new List<string> { "SearchId parameter is required" }
                ));
            }

            var result = await auditLogSearchService.ProcessSearchAsync(tenantFilter, searchId, cancellationToken);

            var message = result.Status switch {
                "Completed" => $"Search '{result.DisplayName}' completed successfully. Found {result.MatchedLogs} matching logs out of {result.TotalLogs} total logs.",
                "Failed" => $"Search '{result.DisplayName}' failed to process: {result.ErrorMessage}",
                "Processing" => $"Search '{result.DisplayName}' is being processed.",
                _ => $"Search '{result.DisplayName}' status: {result.Status}"
            };

            return Results.Ok(Response<AuditLogSearchDto>.SuccessResult(result, message));
        } catch (InvalidOperationException ex) {
            return Results.NotFound(Response<AuditLogSearchDto>.ErrorResult(
                ex.Message,
                new List<string> { "Search not found" }
            ));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error processing audit log search"
            );
        }
    }
}