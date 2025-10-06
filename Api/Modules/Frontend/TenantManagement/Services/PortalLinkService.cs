using CIPP.Api.Data;
using CIPP.Api.Modules.Frontend.TenantManagement.Interfaces;
using CIPP.Api.Modules.Frontend.TenantManagement.Models;
using CIPP.Api.Modules.Tenants.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace CIPP.Api.Modules.Frontend.TenantManagement.Services;

public class PortalLinkService : IPortalLinkService {
    private readonly ApplicationDbContext _context;
    private readonly IMemoryCache _cache;
    private readonly ILogger<PortalLinkService> _logger;
    private readonly TimeSpan _cacheExpiry = TimeSpan.FromHours(24);

    public PortalLinkService(
        ApplicationDbContext context,
        IMemoryCache cache,
        ILogger<PortalLinkService> logger) {
        _context = context;
        _cache = cache;
        _logger = logger;
    }

    public async Task<PortalLinks> GeneratePortalLinksAsync(Guid tenantId, CancellationToken cancellationToken = default) {
        var tenant = await _context.GetEntitySet<Tenant>()
            .FirstOrDefaultAsync(t => t.Id == tenantId, cancellationToken);

        if (tenant == null) {
            throw new InvalidOperationException($"Tenant with ID {tenantId} not found");
        }

        var portalLinks = GeneratePortalLinks(tenant);
        
        var cacheKey = $"portal_links_{tenantId}";
        _cache.Set(cacheKey, portalLinks, _cacheExpiry);
        
        _logger.LogDebug("Generated and cached portal links for tenant {TenantId}", tenant.TenantId);
        return portalLinks;
    }

    public async Task<PortalLinks> GetCachedPortalLinksAsync(Guid tenantId, CancellationToken cancellationToken = default) {
        var cacheKey = $"portal_links_{tenantId}";
        
        if (_cache.TryGetValue(cacheKey, out PortalLinks? cachedLinks) && cachedLinks != null) {
            _logger.LogDebug("Retrieved portal links from cache for tenant {TenantId}", tenantId);
            return cachedLinks;
        }

        return await GeneratePortalLinksAsync(tenantId, cancellationToken);
    }

    public async Task<string> GetPortalUrlAsync(Guid tenantId, string portalType, CancellationToken cancellationToken = default) {
        var portalLinks = await GetCachedPortalLinksAsync(tenantId, cancellationToken);

        var url = portalType.ToLowerInvariant() switch {
            "microsoft365" or "m365" => portalLinks.Microsoft365,
            "exchange" => portalLinks.Exchange,
            "entra" or "azure ad" or "azuread" => portalLinks.Entra,
            "teams" => portalLinks.Teams,
            "azure" => portalLinks.Azure,
            "intune" => portalLinks.Intune,
            "security" => portalLinks.Security,
            "compliance" => portalLinks.Compliance,
            "sharepoint" => portalLinks.SharePoint,
            "powerplatform" or "power platform" => portalLinks.PowerPlatform,
            "powerbi" or "power bi" => portalLinks.PowerBI,
            _ => throw new ArgumentException($"Unknown portal type: {portalType}", nameof(portalType))
        };

        _logger.LogDebug("Retrieved {PortalType} portal URL for tenant {TenantId}", portalType, tenantId);
        return url;
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