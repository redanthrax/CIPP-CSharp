using CIPP.Api.Modules.AuditLogs.Interfaces;
using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.AuditLogs;

namespace CIPP.Api.Modules.AuditLogs.Endpoints;

public static class CreateAuditLogSearch {
    public static void MapCreateAuditLogSearch(this RouteGroupBuilder group) {
        group.MapPost("/audit-log-searches", Handle)
            .WithName("CreateAuditLogSearch")
            .WithSummary("Create audit log search")
            .WithDescription("Creates a new audit log search with specified criteria")
            .RequirePermission("Tenant.Alert.ReadWrite", "Create audit log searches");
    }

    private static async Task<IResult> Handle(
        IAuditLogSearchService auditLogSearchService,
        CreateAuditLogSearchDto request,
        CancellationToken cancellationToken = default) {
        
        try {
            if (string.IsNullOrEmpty(request.TenantFilter)) {
                return Results.BadRequest(Response<AuditLogSearchDto>.ErrorResult(
                    "TenantFilter is required",
                    new List<string> { "TenantFilter is required in request body" }
                ));
            }

            if (request.StartTime >= request.EndTime) {
                return Results.BadRequest(Response<AuditLogSearchDto>.ErrorResult(
                    "Invalid date range",
                    new List<string> { "StartTime must be before EndTime" }
                ));
            }

            var result = await auditLogSearchService.CreateSearchAsync(request, cancellationToken);

            return Results.Ok(Response<AuditLogSearchDto>.SuccessResult(
                result, 
                $"Created audit log search: {result.DisplayName}"
            ));
        } catch (ArgumentException ex) {
            return Results.BadRequest(Response<AuditLogSearchDto>.ErrorResult(
                ex.Message,
                new List<string> { "Invalid request parameters" }
            ));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error creating audit log search"
            );
        }
    }
}