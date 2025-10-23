using CIPP.Api.Modules.AuditLogs.Interfaces;
using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.AuditLogs;

namespace CIPP.Api.Modules.AuditLogs.Endpoints;

public static class GetAuditLogSearchResults {
    public static void MapGetAuditLogSearchResults(this RouteGroupBuilder group) {
        group.MapGet("/audit-log-search-results", Handle)
            .WithName("GetAuditLogSearchResults")
            .WithSummary("Get audit log search results")
            .WithDescription("Returns results from a specific audit log search. " +
                            "Parameters: tenantFilter (required), searchId (required)")
            .RequirePermission("Tenant.Alert.Read", "View audit log search results");
    }

    private static async Task<IResult> Handle(
        IAuditLogSearchService auditLogSearchService,
        string tenantFilter,
        string searchId,
        CancellationToken cancellationToken = default) {
        
        try {
            if (string.IsNullOrEmpty(tenantFilter)) {
                return Results.BadRequest(Response<AuditLogSearchResultDto>.ErrorResult(
                    "TenantFilter is required",
                    new List<string> { "TenantFilter parameter is required" }
                ));
            }

            if (string.IsNullOrEmpty(searchId)) {
                return Results.BadRequest(Response<AuditLogSearchResultDto>.ErrorResult(
                    "SearchId is required",
                    new List<string> { "SearchId parameter is required" }
                ));
            }

            var result = await auditLogSearchService.GetSearchResultsAsync(tenantFilter, searchId, cancellationToken);

            return Results.Ok(Response<AuditLogSearchResultDto>.SuccessResult(result, "Audit log search results retrieved successfully"));
        } catch (InvalidOperationException ex) {
            return Results.NotFound(Response<AuditLogSearchResultDto>.ErrorResult(
                ex.Message,
                new List<string> { "Search not found" }
            ));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving audit log search results"
            );
        }
    }
}