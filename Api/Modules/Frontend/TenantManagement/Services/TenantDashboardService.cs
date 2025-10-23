using CIPP.Api.Data;
using CIPP.Api.Modules.Frontend.TenantManagement.Interfaces;
using CIPP.Api.Modules.Frontend.TenantManagement.Models;
using CIPP.Api.Modules.Tenants.Models;
using CIPP.Api.Modules.MsGraph.Interfaces;
using CIPP.Api.Modules.MsGraph.Services;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CIPP.Api.Modules.Frontend.TenantManagement.Services;

public class TenantDashboardService : ITenantDashboardService {
    private readonly ApplicationDbContext _context;
    private readonly GraphUserService _graphUserService;
    private readonly IMicrosoftGraphService _graphService;
    private readonly ILogger<TenantDashboardService> _logger;

    public TenantDashboardService(
        ApplicationDbContext context,
        GraphUserService graphUserService,
        IMicrosoftGraphService graphService,
        ILogger<TenantDashboardService> logger) {
        _context = context;
        _graphUserService = graphUserService;
        _graphService = graphService;
        _logger = logger;
    }

    public async Task<TenantDashboardData> GetTenantDashboardDataAsync(Guid tenantId, CancellationToken cancellationToken = default) {
        var tenant = await _context.GetEntitySet<Tenant>()
            .FirstOrDefaultAsync(t => t.Id == tenantId, cancellationToken);

        if (tenant == null) {
            throw new InvalidOperationException($"Tenant with ID {tenantId} not found");
        }

        return new TenantDashboardData {
            TenantId = tenant.Id,
            DisplayName = tenant.DisplayName,
            HealthStatus = await GetTenantHealthStatusAsync(tenantId, cancellationToken),
            ActiveUsers = await GetActiveUsersCountAsync(tenant.TenantId, cancellationToken),
            TotalLicenses = GetTotalLicensesFromCapabilities(tenant.Capabilities),
            UsedLicenses = await GetUsedLicensesCountAsync(tenant.TenantId, cancellationToken),
            ActiveAlerts = await GetActiveAlertsCountAsync(tenantId, cancellationToken),
            StandardsCompliance = await GetStandardsComplianceScoreAsync(tenantId, cancellationToken),
            LastHealthCheck = tenant.LastSyncAt ?? DateTime.UtcNow.AddDays(-1),
            Portals = GeneratePortalLinks(tenant)
        };
    }

    public async Task<TenantHealthStatus> GetTenantHealthStatusAsync(Guid tenantId, CancellationToken cancellationToken = default) {
        var tenant = await _context.GetEntitySet<Tenant>()
            .FirstOrDefaultAsync(t => t.Id == tenantId, cancellationToken);

        if (tenant == null) {
            return TenantHealthStatus.Unknown;
        }

        if (tenant.GraphErrorCount > 5) {
            return TenantHealthStatus.Critical;
        }
        
        if (tenant.GraphErrorCount > 0 || tenant.LastSyncAt == null || tenant.LastSyncAt < DateTime.UtcNow.AddDays(-7)) {
            return TenantHealthStatus.Warning;
        }

        return TenantHealthStatus.Healthy;
    }

    public async Task<int> GetActiveUsersCountAsync(string tenantId, CancellationToken cancellationToken = default) {
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

    public async Task<int> GetUsedLicensesCountAsync(string tenantId, CancellationToken cancellationToken = default) {
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

    public async Task RefreshTenantDataAsync(Guid tenantId, CancellationToken cancellationToken = default) {
        var tenant = await _context.GetEntitySet<Tenant>()
            .FirstOrDefaultAsync(t => t.Id == tenantId, cancellationToken);

        if (tenant == null) {
            throw new InvalidOperationException($"Tenant with ID {tenantId} not found");
        }

        try {
            var organization = await _graphService.GetOrganizationAsync(tenant.TenantId);
            if (organization != null) {
                tenant.DisplayName = organization.DisplayName ?? tenant.DisplayName;
                tenant.LastSyncAt = DateTime.UtcNow;
                tenant.GraphErrorCount = 0;

                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Refreshed tenant data for {TenantId}", tenant.TenantId);
            }
        }
        catch (Exception ex) {
            tenant.GraphErrorCount++;
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogError(ex, "Failed to refresh tenant data for {TenantId}", tenant.TenantId);
            throw;
        }
    }

    private static int GetTotalLicensesFromCapabilities(TenantCapabilities? capabilities) {
        return capabilities?.Licenses.Count ?? 0;
    }

    private async Task<int> GetActiveAlertsCountAsync(Guid tenantId, CancellationToken cancellationToken) {
        return await Task.FromResult(0);
    }

    private async Task<int> GetStandardsComplianceScoreAsync(Guid tenantId, CancellationToken cancellationToken) {
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