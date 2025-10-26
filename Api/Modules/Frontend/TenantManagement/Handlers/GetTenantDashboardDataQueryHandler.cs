using CIPP.Api.Data;
using CIPP.Api.Modules.Frontend.TenantManagement.Models;
using CIPP.Api.Modules.Frontend.TenantManagement.Queries;
using CIPP.Api.Modules.Tenants.Models;
using CIPP.Api.Modules.MsGraph.Interfaces;
using CIPP.Api.Modules.MsGraph.Services;
using DispatchR.Abstractions.Send;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CIPP.Api.Modules.Frontend.TenantManagement.Handlers;

public class GetTenantDashboardDataQueryHandler : IRequestHandler<GetTenantDashboardDataQuery, Task<TenantDashboardData>> {
    private readonly ApplicationDbContext _context;
    private readonly GraphUserService _graphUserService;
    private readonly ILogger<GetTenantDashboardDataQueryHandler> _logger;

    public GetTenantDashboardDataQueryHandler(
        ApplicationDbContext context, 
        GraphUserService graphUserService,
        ILogger<GetTenantDashboardDataQueryHandler> logger) {
        _context = context;
        _graphUserService = graphUserService;
        _logger = logger;
    }

    public async Task<TenantDashboardData> Handle(GetTenantDashboardDataQuery request, CancellationToken cancellationToken) {
        try {
            var tenant = await _context.GetEntitySet<Tenant>()
                .FirstOrDefaultAsync(t => t.TenantId == request.TenantId, cancellationToken);

            if (tenant == null) {
                throw new InvalidOperationException($"Tenant with ID {request.TenantId} not found");
            }

            var dashboardData = new TenantDashboardData {
                TenantId = tenant.TenantId,
                DisplayName = tenant.DisplayName,
                HealthStatus = DetermineHealthStatus(tenant),
                ActiveUsers = await GetActiveUsersCount(tenant.TenantId, cancellationToken),
                TotalLicenses = GetTotalLicensesFromCapabilities(tenant.Capabilities),
                UsedLicenses = await GetUsedLicensesCount(tenant.TenantId, cancellationToken),
                ActiveAlerts = await GetActiveAlertsCount(tenant.TenantId, cancellationToken),
                StandardsCompliance = await GetStandardsComplianceScore(tenant.TenantId, cancellationToken),
                LastHealthCheck = tenant.LastSyncAt ?? DateTime.UtcNow.AddDays(-1),
                Portals = GeneratePortalLinks(tenant)
            };

            _logger.LogDebug("Generated dashboard data for tenant {TenantId}", tenant.TenantId);
            return dashboardData;
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error generating dashboard data for tenant {TenantId}", request.TenantId);
            throw;
        }
    }

    private static TenantHealthStatus DetermineHealthStatus(Tenant tenant) {
        if (tenant.GraphErrorCount > 5) {
            return TenantHealthStatus.Critical;
        }
        
        if (tenant.GraphErrorCount > 0 || tenant.LastSyncAt == null || tenant.LastSyncAt < DateTime.UtcNow.AddDays(-7)) {
            return TenantHealthStatus.Warning;
        }

        return TenantHealthStatus.Healthy;
    }

    private async Task<int> GetActiveUsersCount(Guid tenantId, CancellationToken cancellationToken) {
        try {
            var usersResponse = await _graphUserService.ListUsersAsync(tenantId, 
                filter: "accountEnabled eq true and userType eq 'Member'",
                select: new[] { "id" });
            
            return usersResponse?.Value?.Count ?? 0;
        }
        catch (Exception ex) {
            _logger.LogWarning(ex, "Failed to get active users count for tenant {TenantId}", tenantId);
            return 0;
        }
    }

    private static int GetTotalLicensesFromCapabilities(TenantCapabilities? capabilities) {
        return capabilities?.Licenses.Count ?? 0;
    }

    private async Task<int> GetUsedLicensesCount(Guid tenantId, CancellationToken cancellationToken) {
        try {
            var usersResponse = await _graphUserService.ListUsersAsync(tenantId,
                filter: "assignedLicenses/any()",
                select: new[] { "id" });
            
            return usersResponse?.Value?.Count ?? 0;
        }
        catch (Exception ex) {
            _logger.LogWarning(ex, "Failed to get used licenses count for tenant {TenantId}", tenantId);
            return 0;
        }
    }

    private async Task<int> GetActiveAlertsCount(Guid tenantId, CancellationToken cancellationToken) {
        return await Task.FromResult(0);
    }

    private async Task<int> GetStandardsComplianceScore(Guid tenantId, CancellationToken cancellationToken) {
        return await Task.FromResult(100);
    }


    private static PortalLinks GeneratePortalLinks(Tenant tenant) {
        var tenantId = tenant.TenantId;
        var domain = tenant.DefaultDomainName;
        var domainPrefix = domain.Split('.')[0];

        return new PortalLinks {
            Microsoft365 = $"https://admin.microsoft.com/?tenantId={tenantId}",
            Exchange = $"https://admin.exchange.microsoft.com/?tenantId={tenantId}",
            Entra = $"https://entra.microsoft.com/{domain}",
            Teams = $"https://admin.teams.microsoft.com/?tenantId={tenantId}",
            Azure = $"https://portal.azure.com/{tenantId}",
            Intune = $"https://endpoint.microsoft.com/?tenantId={tenantId}",
            Security = $"https://security.microsoft.com/?tenantId={tenantId}",
            Compliance = $"https://compliance.microsoft.com/?tenantId={tenantId}",
            SharePoint = $"https://{domainPrefix}-admin.sharepoint.com",
            PowerPlatform = $"https://admin.powerplatform.microsoft.com/environments?tenantId={tenantId}",
            PowerBI = $"https://app.powerbi.com/?tenantId={tenantId}"
        };
    }
}