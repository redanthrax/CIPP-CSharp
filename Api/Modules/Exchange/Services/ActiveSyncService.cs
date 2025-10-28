using CIPP.Api.Extensions;
using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;

namespace CIPP.Api.Modules.Exchange.Services;

public class ActiveSyncService : IActiveSyncService {
    private readonly IExchangeOnlineService _exoService;
    private readonly ILogger<ActiveSyncService> _logger;

    public ActiveSyncService(IExchangeOnlineService exoService, ILogger<ActiveSyncService> logger) {
        _exoService = exoService;
        _logger = logger;
    }

    public async Task<PagedResponse<ActiveSyncDeviceAccessRuleDto>> GetDeviceAccessRulesAsync(Guid tenantId, PagingParameters pagingParams, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting ActiveSync device access rules for tenant {TenantId}", tenantId);
        
        var rules = await _exoService.ExecuteCmdletListAsync<ActiveSyncDeviceAccessRuleDto>(
            tenantId,
            "Get-ActiveSyncDeviceAccessRule",
            null,
            cancellationToken
        );

        foreach (var rule in rules) {
            rule.TenantId = tenantId;
        }

        return rules.ToPagedResponse(pagingParams);
    }

    public async Task<ActiveSyncDeviceAccessRuleDto?> GetDeviceAccessRuleAsync(Guid tenantId, string ruleIdentity, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting ActiveSync device access rule {RuleIdentity} for tenant {TenantId}", ruleIdentity, tenantId);
        
        var parameters = new Dictionary<string, object> {
            { "Identity", ruleIdentity }
        };

        var rule = await _exoService.ExecuteCmdletAsync<ActiveSyncDeviceAccessRuleDto>(
            tenantId,
            "Get-ActiveSyncDeviceAccessRule",
            parameters,
            cancellationToken
        );

        if (rule != null) {
            rule.TenantId = tenantId;
        }

        return rule;
    }

    public async Task<string> CreateDeviceAccessRuleAsync(Guid tenantId, CreateActiveSyncDeviceAccessRuleDto createDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Creating ActiveSync device access rule for tenant {TenantId}", tenantId);
        
        var parameters = new Dictionary<string, object> {
            { "QueryString", createDto.QueryString },
            { "Characteristic", createDto.Characteristic },
            { "AccessLevel", createDto.AccessLevel }
        };

        await _exoService.ExecuteCmdletNoResultAsync(
            tenantId,
            "New-ActiveSyncDeviceAccessRule",
            parameters,
            cancellationToken
        );

        _logger.LogInformation("Successfully created ActiveSync device access rule");
        return $"Rule created for {createDto.Characteristic}: {createDto.QueryString}";
    }

    public async Task UpdateDeviceAccessRuleAsync(Guid tenantId, string ruleIdentity, UpdateActiveSyncDeviceAccessRuleDto updateDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Updating ActiveSync device access rule {RuleIdentity} for tenant {TenantId}", ruleIdentity, tenantId);
        
        var parameters = new Dictionary<string, object> {
            { "Identity", ruleIdentity }
        };

        if (!string.IsNullOrEmpty(updateDto.QueryString))
            parameters.Add("QueryString", updateDto.QueryString);

        if (!string.IsNullOrEmpty(updateDto.Characteristic))
            parameters.Add("Characteristic", updateDto.Characteristic);

        if (!string.IsNullOrEmpty(updateDto.AccessLevel))
            parameters.Add("AccessLevel", updateDto.AccessLevel);

        await _exoService.ExecuteCmdletNoResultAsync(
            tenantId,
            "Set-ActiveSyncDeviceAccessRule",
            parameters,
            cancellationToken
        );

        _logger.LogInformation("Successfully updated ActiveSync device access rule {RuleIdentity}", ruleIdentity);
    }

    public async Task DeleteDeviceAccessRuleAsync(Guid tenantId, string ruleIdentity, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Deleting ActiveSync device access rule {RuleIdentity} from tenant {TenantId}", ruleIdentity, tenantId);
        
        var parameters = new Dictionary<string, object> {
            { "Identity", ruleIdentity },
            { "Confirm", false }
        };

        await _exoService.ExecuteCmdletNoResultAsync(
            tenantId,
            "Remove-ActiveSyncDeviceAccessRule",
            parameters,
            cancellationToken
        );

        _logger.LogInformation("Successfully deleted ActiveSync device access rule {RuleIdentity}", ruleIdentity);
    }
}
