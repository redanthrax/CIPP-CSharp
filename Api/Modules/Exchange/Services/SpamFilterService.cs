using CIPP.Api.Extensions;
using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;

namespace CIPP.Api.Modules.Exchange.Services;

public class SpamFilterService : ISpamFilterService {
    private readonly IExchangeOnlineService _exoService;
    private readonly ILogger<SpamFilterService> _logger;

    public SpamFilterService(IExchangeOnlineService exoService, ILogger<SpamFilterService> logger) {
        _exoService = exoService;
        _logger = logger;
    }

    public async Task<PagedResponse<HostedContentFilterPolicyDto>> GetAntiSpamPoliciesAsync(Guid tenantId, PagingParameters pagingParams, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting anti-spam policies for tenant {TenantId}", tenantId);
        
        var policies = await _exoService.ExecuteCmdletListAsync<HostedContentFilterPolicyDto>(
            tenantId,
            "Get-HostedContentFilterPolicy",
            null,
            cancellationToken
        );

        return policies.ToPagedResponse(pagingParams);
    }

    public async Task<HostedContentFilterPolicyDto?> GetAntiSpamPolicyAsync(Guid tenantId, string policyId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting anti-spam policy {PolicyId} for tenant {TenantId}", policyId, tenantId);
        
        var parameters = new Dictionary<string, object> {
            { "Identity", policyId }
        };

        var policy = await _exoService.ExecuteCmdletAsync<HostedContentFilterPolicyDto>(
            tenantId,
            "Get-HostedContentFilterPolicy",
            parameters,
            cancellationToken
        );

        return policy;
    }

    public async Task UpdateAntiSpamPolicyAsync(Guid tenantId, string policyId, UpdateHostedContentFilterPolicyDto updateDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Updating anti-spam policy {PolicyId} for tenant {TenantId}", policyId, tenantId);
        
        var parameters = BuildAntiSpamUpdateParameters(policyId, updateDto);

        await _exoService.ExecuteCmdletNoResultAsync(
            tenantId,
            "Set-HostedContentFilterPolicy",
            parameters,
            cancellationToken
        );

        _logger.LogInformation("Successfully updated anti-spam policy {PolicyId}", policyId);
    }

    public async Task<PagedResponse<MalwareFilterPolicyDto>> GetAntiMalwarePoliciesAsync(Guid tenantId, PagingParameters pagingParams, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting anti-malware policies for tenant {TenantId}", tenantId);
        
        var policies = await _exoService.ExecuteCmdletListAsync<MalwareFilterPolicyDto>(
            tenantId,
            "Get-MalwareFilterPolicy",
            null,
            cancellationToken
        );

        return policies.ToPagedResponse(pagingParams);
    }

    public async Task<MalwareFilterPolicyDto?> GetAntiMalwarePolicyAsync(Guid tenantId, string policyId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting anti-malware policy {PolicyId} for tenant {TenantId}", policyId, tenantId);
        
        var parameters = new Dictionary<string, object> {
            { "Identity", policyId }
        };

        var policy = await _exoService.ExecuteCmdletAsync<MalwareFilterPolicyDto>(
            tenantId,
            "Get-MalwareFilterPolicy",
            parameters,
            cancellationToken
        );

        return policy;
    }

    public async Task UpdateAntiMalwarePolicyAsync(Guid tenantId, string policyId, UpdateMalwareFilterPolicyDto updateDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Updating anti-malware policy {PolicyId} for tenant {TenantId}", policyId, tenantId);
        
        var parameters = BuildAntiMalwareUpdateParameters(policyId, updateDto);

        await _exoService.ExecuteCmdletNoResultAsync(
            tenantId,
            "Set-MalwareFilterPolicy",
            parameters,
            cancellationToken
        );

        _logger.LogInformation("Successfully updated anti-malware policy {PolicyId}", policyId);
    }

    public async Task<PagedResponse<QuarantineMessageDto>> GetQuarantineMessagesAsync(Guid tenantId, PagingParameters pagingParams, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting quarantine messages for tenant {TenantId}", tenantId);
        
        var parameters = new Dictionary<string, object> {
            { "PageSize", pagingParams.PageSize }
        };

        var messages = await _exoService.ExecuteCmdletListAsync<QuarantineMessageDto>(
            tenantId,
            "Get-QuarantineMessage",
            parameters,
            cancellationToken
        );

        return messages.ToPagedResponse(pagingParams);
    }

    public async Task ReleaseQuarantineMessageAsync(Guid tenantId, string messageId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Releasing quarantine message {MessageId} for tenant {TenantId}", messageId, tenantId);
        
        var parameters = new Dictionary<string, object> {
            { "Identity", messageId },
            { "ReleaseToAll", true }
        };

        await _exoService.ExecuteCmdletNoResultAsync(
            tenantId,
            "Release-QuarantineMessage",
            parameters,
            cancellationToken
        );

        _logger.LogInformation("Successfully released quarantine message {MessageId}", messageId);
    }

    public async Task DeleteQuarantineMessageAsync(Guid tenantId, string messageId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Deleting quarantine message {MessageId} for tenant {TenantId}", messageId, tenantId);
        
        var parameters = new Dictionary<string, object> {
            { "Identity", messageId }
        };

        await _exoService.ExecuteCmdletNoResultAsync(
            tenantId,
            "Delete-QuarantineMessage",
            parameters,
            cancellationToken
        );

        _logger.LogInformation("Successfully deleted quarantine message {MessageId}", messageId);
    }

    private Dictionary<string, object> BuildAntiSpamUpdateParameters(string policyId, UpdateHostedContentFilterPolicyDto updateDto) {
        var parameters = new Dictionary<string, object> {
            { "Identity", policyId }
        };

        if (!string.IsNullOrEmpty(updateDto.AdminDisplayName))
            parameters.Add("AdminDisplayName", updateDto.AdminDisplayName);

        if (updateDto.BulkThreshold.HasValue)
            parameters.Add("BulkThreshold", updateDto.BulkThreshold.Value);

        if (updateDto.AllowedSenders?.Any() == true)
            parameters.Add("AllowedSenders", updateDto.AllowedSenders.ToArray());

        if (updateDto.AllowedSenderDomains?.Any() == true)
            parameters.Add("AllowedSenderDomains", updateDto.AllowedSenderDomains.ToArray());

        if (updateDto.BlockedSenders?.Any() == true)
            parameters.Add("BlockedSenders", updateDto.BlockedSenders.ToArray());

        if (updateDto.BlockedSenderDomains?.Any() == true)
            parameters.Add("BlockedSenderDomains", updateDto.BlockedSenderDomains.ToArray());

        if (updateDto.EnableEndUserSpamNotifications.HasValue)
            parameters.Add("EnableEndUserSpamNotifications", updateDto.EnableEndUserSpamNotifications.Value);

        if (updateDto.EndUserSpamNotificationFrequency.HasValue)
            parameters.Add("EndUserSpamNotificationFrequency", updateDto.EndUserSpamNotificationFrequency.Value);

        if (!string.IsNullOrEmpty(updateDto.SpamAction))
            parameters.Add("SpamAction", updateDto.SpamAction);

        if (!string.IsNullOrEmpty(updateDto.HighConfidenceSpamAction))
            parameters.Add("HighConfidenceSpamAction", updateDto.HighConfidenceSpamAction);

        if (!string.IsNullOrEmpty(updateDto.PhishSpamAction))
            parameters.Add("PhishSpamAction", updateDto.PhishSpamAction);

        if (!string.IsNullOrEmpty(updateDto.HighConfidencePhishAction))
            parameters.Add("HighConfidencePhishAction", updateDto.HighConfidencePhishAction);

        if (!string.IsNullOrEmpty(updateDto.BulkSpamAction))
            parameters.Add("BulkSpamAction", updateDto.BulkSpamAction);

        if (updateDto.MarkAsSpamBulkMail.HasValue)
            parameters.Add("MarkAsSpamBulkMail", updateDto.MarkAsSpamBulkMail.Value);

        if (updateDto.IncreaseScoreWithImageLinks.HasValue)
            parameters.Add("IncreaseScoreWithImageLinks", updateDto.IncreaseScoreWithImageLinks.Value);

        if (updateDto.IncreaseScoreWithNumericIps.HasValue)
            parameters.Add("IncreaseScoreWithNumericIps", updateDto.IncreaseScoreWithNumericIps.Value);

        if (updateDto.IncreaseScoreWithRedirectToOtherPort.HasValue)
            parameters.Add("IncreaseScoreWithRedirectToOtherPort", updateDto.IncreaseScoreWithRedirectToOtherPort.Value);

        if (updateDto.IncreaseScoreWithBizOrInfoUrls.HasValue)
            parameters.Add("IncreaseScoreWithBizOrInfoUrls", updateDto.IncreaseScoreWithBizOrInfoUrls.Value);

        if (updateDto.QuarantineRetentionPeriod.HasValue)
            parameters.Add("QuarantineRetentionPeriod", updateDto.QuarantineRetentionPeriod.Value);

        return parameters;
    }

    private Dictionary<string, object> BuildAntiMalwareUpdateParameters(string policyId, UpdateMalwareFilterPolicyDto updateDto) {
        var parameters = new Dictionary<string, object> {
            { "Identity", policyId }
        };

        if (!string.IsNullOrEmpty(updateDto.AdminDisplayName))
            parameters.Add("AdminDisplayName", updateDto.AdminDisplayName);

        if (!string.IsNullOrEmpty(updateDto.Action))
            parameters.Add("Action", updateDto.Action);

        if (updateDto.EnableFileFilter.HasValue)
            parameters.Add("EnableFileFilter", updateDto.EnableFileFilter.Value);

        if (updateDto.EnableInternalSenderAdminNotifications.HasValue)
            parameters.Add("EnableInternalSenderAdminNotifications", updateDto.EnableInternalSenderAdminNotifications.Value);

        if (updateDto.EnableInternalSenderNotifications.HasValue)
            parameters.Add("EnableInternalSenderNotifications", updateDto.EnableInternalSenderNotifications.Value);

        if (updateDto.EnableExternalSenderAdminNotifications.HasValue)
            parameters.Add("EnableExternalSenderAdminNotifications", updateDto.EnableExternalSenderAdminNotifications.Value);

        if (updateDto.EnableExternalSenderNotifications.HasValue)
            parameters.Add("EnableExternalSenderNotifications", updateDto.EnableExternalSenderNotifications.Value);

        if (updateDto.InternalSenderAdminAddress?.Any() == true)
            parameters.Add("InternalSenderAdminAddress", updateDto.InternalSenderAdminAddress.ToArray());

        if (updateDto.ExternalSenderAdminAddress?.Any() == true)
            parameters.Add("ExternalSenderAdminAddress", updateDto.ExternalSenderAdminAddress.ToArray());

        if (updateDto.FileTypes?.Any() == true)
            parameters.Add("FileTypes", updateDto.FileTypes.ToArray());

        if (updateDto.ZapEnabled.HasValue)
            parameters.Add("ZapEnabled", updateDto.ZapEnabled.Value);

        if (!string.IsNullOrEmpty(updateDto.CustomNotifications))
            parameters.Add("CustomNotifications", updateDto.CustomNotifications);

        return parameters;
    }
}
