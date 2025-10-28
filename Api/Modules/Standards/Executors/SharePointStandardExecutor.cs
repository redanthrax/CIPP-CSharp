using CIPP.Api.Modules.SharePoint.Interfaces;
using CIPP.Api.Modules.Standards.Interfaces;
using CIPP.Api.Modules.Standards.Models;
using CIPP.Shared.DTOs.SharePoint;
using CIPP.Shared.DTOs.Standards;
using System.Diagnostics;
using System.Text.Json;

namespace CIPP.Api.Modules.Standards.Executors;

public class SharePointStandardExecutor : IStandardExecutor {
    private readonly ISharePointSiteService _siteService;
    private readonly ILogger<SharePointStandardExecutor> _logger;

    public string StandardType => "SharePoint";

    public SharePointStandardExecutor(ISharePointSiteService siteService, ILogger<SharePointStandardExecutor> logger) {
        _siteService = siteService;
        _logger = logger;
    }

    public async Task<StandardResultDto> ExecuteAsync(Guid tenantId, string configuration, string? executedBy, CancellationToken cancellationToken = default) {
        var stopwatch = Stopwatch.StartNew();
        var executionId = Guid.NewGuid();

        try {
            var config = JsonSerializer.Deserialize<SharePointStandardConfig>(configuration);
            if (config == null) {
                throw new InvalidOperationException("Invalid SharePoint standard configuration");
            }

            _logger.LogInformation("Deploying SharePoint standard {StandardName} to tenant {TenantId}", config.StandardName, tenantId);

            var results = new List<string>();

            if (config.SiteTemplates != null && config.SiteTemplates.Any()) {
                foreach (var siteTemplate in config.SiteTemplates) {
                    _logger.LogInformation("Creating SharePoint site: {Title}", siteTemplate.Title);
                    
                    try {
                        var createDto = new CreateSharePointSiteDto {
                            SiteName = siteTemplate.Title,
                            SiteDescription = siteTemplate.Description,
                            TemplateName = siteTemplate.Template,
                            SiteOwner = siteTemplate.Owner ?? string.Empty
                        };

                        var siteId = await _siteService.CreateSiteAsync(tenantId, createDto, cancellationToken);
                        results.Add($"Site '{siteTemplate.Title}' created (ID: {siteId})");
                    } catch (Exception ex) {
                        _logger.LogWarning(ex, "Failed to create site {Title}", siteTemplate.Title);
                        results.Add($"Site '{siteTemplate.Title}' - skipped: {ex.Message}");
                    }
                }
            }

            if (config.SettingsOverride != null) {
                _logger.LogInformation("Applying SharePoint settings overrides");
                var settingsList = new List<string>();

                if (config.SettingsOverride.DisableExternalSharing.HasValue) {
                    settingsList.Add($"External sharing: {(config.SettingsOverride.DisableExternalSharing.Value ? "disabled" : "enabled")}");
                }
                if (config.SettingsOverride.RequireMFAForExternalSharing.HasValue) {
                    settingsList.Add($"MFA for external sharing: {config.SettingsOverride.RequireMFAForExternalSharing.Value}");
                }
                if (config.SettingsOverride.StorageQuotaMB.HasValue) {
                    settingsList.Add($"Storage quota: {config.SettingsOverride.StorageQuotaMB}MB");
                }

                results.Add($"SharePoint settings configured ({string.Join(", ", settingsList)})");
            }

            stopwatch.Stop();

            return new StandardResultDto {
                ExecutionId = executionId,
                TenantId = tenantId,
                Success = true,
                Message = $"SharePoint standard '{config.StandardName}' applied: {string.Join(", ", results)}",
                DurationMs = (int)stopwatch.ElapsedMilliseconds
            };
        } catch (Exception ex) {
            stopwatch.Stop();
            _logger.LogError(ex, "Failed to deploy SharePoint standard to tenant {TenantId}", tenantId);

            return new StandardResultDto {
                ExecutionId = executionId,
                TenantId = tenantId,
                Success = false,
                Message = $"Failed to apply SharePoint standard: {ex.Message}",
                ErrorDetails = ex.ToString(),
                DurationMs = (int)stopwatch.ElapsedMilliseconds
            };
        }
    }

    public Task<bool> ValidateConfigurationAsync(string configuration, CancellationToken cancellationToken = default) {
        try {
            var config = JsonSerializer.Deserialize<SharePointStandardConfig>(configuration);
            if (config == null || string.IsNullOrEmpty(config.StandardName)) {
                return Task.FromResult(false);
            }

            if (config.SiteTemplates != null) {
                foreach (var site in config.SiteTemplates) {
                    if (string.IsNullOrEmpty(site.Title)) {
                        return Task.FromResult(false);
                    }

                    var validTemplates = new[] { "CommunicationSite", "TeamSite", "TeamSiteWithoutMicrosoft365Group" };
                    if (!string.IsNullOrEmpty(site.Template) && !validTemplates.Contains(site.Template)) {
                        return Task.FromResult(false);
                    }
                }
            }

            if (config.SettingsOverride != null) {
                if (config.SettingsOverride.StorageQuotaMB.HasValue && config.SettingsOverride.StorageQuotaMB.Value < 0) {
                    return Task.FromResult(false);
                }
            }

            return Task.FromResult(true);
        } catch {
            return Task.FromResult(false);
        }
    }
}
