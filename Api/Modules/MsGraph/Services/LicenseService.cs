using CIPP.Api.Modules.MsGraph.Interfaces;
using Microsoft.Graph.Beta.Models;
using Microsoft.Graph.Beta.Users.Item.AssignLicense;

namespace CIPP.Api.Modules.MsGraph.Services;

public class LicenseService : ILicenseService {
    private readonly IMicrosoftGraphService _graphService;
    private readonly ILogger<LicenseService> _logger;

    public LicenseService(IMicrosoftGraphService graphService, ILogger<LicenseService> logger) {
        _graphService = graphService;
        _logger = logger;
    }

    public async Task AssignLicensesAsync(string tenantId, string userId, List<string> licenseSkuIds, CancellationToken cancellationToken = default) {
        if (!licenseSkuIds.Any()) return;

        _logger.LogInformation("Assigning {LicenseCount} licenses to user {UserId} in tenant {TenantId}",
            licenseSkuIds.Count, userId, tenantId);

        try {
            var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
            
            var assignLicenses = new AssignLicensePostRequestBody {
                AddLicenses = licenseSkuIds.Select(skuId => new AssignedLicense {
                    SkuId = Guid.Parse(skuId)
                }).ToList(),
                RemoveLicenses = new List<Guid?>()
            };

            await graphClient.Users[userId].AssignLicense.PostAsync(assignLicenses, null, cancellationToken);
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to assign licenses to user {UserId}", userId);
            throw new InvalidOperationException($"Failed to assign licenses: {ex.Message}", ex);
        }
    }

    public async Task RemoveLicensesAsync(string tenantId, string userId, List<string> licenseSkuIds, CancellationToken cancellationToken = default) {
        if (!licenseSkuIds.Any()) return;

        _logger.LogInformation("Removing {LicenseCount} licenses from user {UserId} in tenant {TenantId}",
            licenseSkuIds.Count, userId, tenantId);

        try {
            var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
            
            var assignLicenses = new AssignLicensePostRequestBody {
                AddLicenses = new List<AssignedLicense>(),
                RemoveLicenses = licenseSkuIds.Select(skuId => (Guid?)Guid.Parse(skuId)).ToList()
            };

            await graphClient.Users[userId].AssignLicense.PostAsync(assignLicenses, null, cancellationToken);
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to remove licenses from user {UserId}", userId);
            throw new InvalidOperationException($"Failed to remove licenses: {ex.Message}", ex);
        }
    }

    public async Task ReplaceLicensesAsync(string tenantId, string userId, List<string> addLicenseSkuIds, List<string> removeLicenseSkuIds, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Replacing licenses for user {UserId} in tenant {TenantId}. Adding {AddCount}, removing {RemoveCount}",
            userId, tenantId, addLicenseSkuIds.Count, removeLicenseSkuIds.Count);

        try {
            var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
            
            var assignLicenses = new AssignLicensePostRequestBody {
                AddLicenses = addLicenseSkuIds.Select(skuId => new AssignedLicense {
                    SkuId = Guid.Parse(skuId)
                }).ToList(),
                RemoveLicenses = removeLicenseSkuIds.Select(skuId => (Guid?)Guid.Parse(skuId)).ToList()
            };

            await graphClient.Users[userId].AssignLicense.PostAsync(assignLicenses, null, cancellationToken);
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to replace licenses for user {UserId}", userId);
            throw new InvalidOperationException($"Failed to replace licenses: {ex.Message}", ex);
        }
    }

    public async Task<List<string>> GetUserLicensesAsync(string tenantId, string userId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting licenses for user {UserId} in tenant {TenantId}", userId, tenantId);

        try {
            var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
            var user = await graphClient.Users[userId].GetAsync(null, cancellationToken);
            
            return user?.AssignedLicenses?.Select(l => l.SkuId?.ToString() ?? "").Where(s => !string.IsNullOrEmpty(s)).ToList() ?? new List<string>();
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to get licenses for user {UserId}", userId);
            throw new InvalidOperationException($"Failed to get user licenses: {ex.Message}", ex);
        }
    }
}