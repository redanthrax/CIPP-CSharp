using CIPP.Api.Extensions;
using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;

namespace CIPP.Api.Modules.Exchange.Services;

public class SafePoliciesService : ISafePoliciesService {
    private readonly IExchangeOnlineService _exoService;
    private readonly ILogger<SafePoliciesService> _logger;

    public SafePoliciesService(IExchangeOnlineService exoService, ILogger<SafePoliciesService> logger) {
        _exoService = exoService;
        _logger = logger;
    }

    public async Task<PagedResponse<SafeLinksPolicyDto>> GetSafeLinksPoliciesAsync(string tenantId, PagingParameters pagingParams, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting Safe Links policies for tenant {TenantId}", tenantId);
        
        var policies = await _exoService.ExecuteCmdletListAsync<SafeLinksPolicyDto>(
            tenantId,
            "Get-SafeLinksPolicy",
            null,
            cancellationToken
        );

        return policies.ToPagedResponse(pagingParams);
    }

    public async Task<SafeLinksPolicyDto?> GetSafeLinksPolicyAsync(string tenantId, string policyName, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting Safe Links policy {PolicyName} for tenant {TenantId}", policyName, tenantId);
        
        var parameters = new Dictionary<string, object> {
            { "Identity", policyName }
        };

        var policy = await _exoService.ExecuteCmdletAsync<SafeLinksPolicyDto>(
            tenantId,
            "Get-SafeLinksPolicy",
            parameters,
            cancellationToken
        );

        return policy;
    }

    public async Task UpdateSafeLinksPolicyAsync(string tenantId, string policyName, UpdateSafeLinksPolicyDto updateDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Updating Safe Links policy {PolicyName} for tenant {TenantId}", policyName, tenantId);
        
        var parameters = BuildSafeLinksUpdateParameters(policyName, updateDto);

        await _exoService.ExecuteCmdletNoResultAsync(
            tenantId,
            "Set-SafeLinksPolicy",
            parameters,
            cancellationToken
        );

        _logger.LogInformation("Successfully updated Safe Links policy {PolicyName}", policyName);
    }

    public async Task<PagedResponse<SafeAttachmentsPolicyDto>> GetSafeAttachmentPoliciesAsync(string tenantId, PagingParameters pagingParams, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting Safe Attachments policies for tenant {TenantId}", tenantId);
        
        var policies = await _exoService.ExecuteCmdletListAsync<SafeAttachmentsPolicyDto>(
            tenantId,
            "Get-SafeAttachmentPolicy",
            null,
            cancellationToken
        );

        return policies.ToPagedResponse(pagingParams);
    }

    public async Task<SafeAttachmentsPolicyDto?> GetSafeAttachmentPolicyAsync(string tenantId, string policyName, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting Safe Attachments policy {PolicyName} for tenant {TenantId}", policyName, tenantId);
        
        var parameters = new Dictionary<string, object> {
            { "Identity", policyName }
        };

        var policy = await _exoService.ExecuteCmdletAsync<SafeAttachmentsPolicyDto>(
            tenantId,
            "Get-SafeAttachmentPolicy",
            parameters,
            cancellationToken
        );

        return policy;
    }

    public async Task UpdateSafeAttachmentPolicyAsync(string tenantId, string policyName, UpdateSafeAttachmentsPolicyDto updateDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Updating Safe Attachments policy {PolicyName} for tenant {TenantId}", policyName, tenantId);
        
        var parameters = BuildSafeAttachmentsUpdateParameters(policyName, updateDto);

        await _exoService.ExecuteCmdletNoResultAsync(
            tenantId,
            "Set-SafeAttachmentPolicy",
            parameters,
            cancellationToken
        );

        _logger.LogInformation("Successfully updated Safe Attachments policy {PolicyName}", policyName);
    }

    public async Task<AtpPolicyDto?> GetAtpPolicyAsync(string tenantId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting ATP policy for tenant {TenantId}", tenantId);
        
        var policy = await _exoService.ExecuteCmdletAsync<AtpPolicyDto>(
            tenantId,
            "Get-AtpPolicyForO365",
            null,
            cancellationToken
        );

        return policy;
    }

    public async Task UpdateAtpPolicyAsync(string tenantId, UpdateAtpPolicyDto updateDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Updating ATP policy for tenant {TenantId}", tenantId);
        
        var parameters = BuildAtpUpdateParameters(updateDto);

        await _exoService.ExecuteCmdletNoResultAsync(
            tenantId,
            "Set-AtpPolicyForO365",
            parameters,
            cancellationToken
        );

        _logger.LogInformation("Successfully updated ATP policy");
    }

    private Dictionary<string, object> BuildSafeLinksUpdateParameters(string policyName, UpdateSafeLinksPolicyDto updateDto) {
        var parameters = new Dictionary<string, object> {
            { "Identity", policyName }
        };

        if (updateDto.EnableSafeLinksForEmail.HasValue)
            parameters.Add("EnableSafeLinksForEmail", updateDto.EnableSafeLinksForEmail.Value);

        if (updateDto.EnableSafeLinksForTeams.HasValue)
            parameters.Add("EnableSafeLinksForTeams", updateDto.EnableSafeLinksForTeams.Value);

        if (updateDto.EnableSafeLinksForOffice.HasValue)
            parameters.Add("EnableSafeLinksForOffice", updateDto.EnableSafeLinksForOffice.Value);

        if (updateDto.TrackClicks.HasValue)
            parameters.Add("TrackClicks", updateDto.TrackClicks.Value);

        if (updateDto.AllowClickThrough.HasValue)
            parameters.Add("AllowClickThrough", updateDto.AllowClickThrough.Value);

        if (updateDto.ScanUrls.HasValue)
            parameters.Add("ScanUrls", updateDto.ScanUrls.Value);

        if (updateDto.EnableForInternalSenders.HasValue)
            parameters.Add("EnableForInternalSenders", updateDto.EnableForInternalSenders.Value);

        if (updateDto.DeliverMessageAfterScan.HasValue)
            parameters.Add("DeliverMessageAfterScan", updateDto.DeliverMessageAfterScan.Value);

        if (updateDto.DisableUrlRewrite.HasValue)
            parameters.Add("DisableUrlRewrite", updateDto.DisableUrlRewrite.Value);

        if (updateDto.EnableOrganizationBranding.HasValue)
            parameters.Add("EnableOrganizationBranding", updateDto.EnableOrganizationBranding.Value);

        if (updateDto.DoNotRewriteUrls?.Any() == true)
            parameters.Add("DoNotRewriteUrls", updateDto.DoNotRewriteUrls.ToArray());

        return parameters;
    }

    private Dictionary<string, object> BuildSafeAttachmentsUpdateParameters(string policyName, UpdateSafeAttachmentsPolicyDto updateDto) {
        var parameters = new Dictionary<string, object> {
            { "Identity", policyName }
        };

        if (updateDto.Enable.HasValue)
            parameters.Add("Enable", updateDto.Enable.Value);

        if (!string.IsNullOrEmpty(updateDto.Action))
            parameters.Add("Action", updateDto.Action);

        if (!string.IsNullOrEmpty(updateDto.QuarantineTag))
            parameters.Add("QuarantineTag", updateDto.QuarantineTag);

        if (updateDto.Redirect.HasValue)
            parameters.Add("Redirect", updateDto.Redirect.Value);

        if (!string.IsNullOrEmpty(updateDto.RedirectAddress))
            parameters.Add("RedirectAddress", updateDto.RedirectAddress);

        return parameters;
    }

    private Dictionary<string, object> BuildAtpUpdateParameters(UpdateAtpPolicyDto updateDto) {
        var parameters = new Dictionary<string, object>();

        if (updateDto.EnableATPForSPOTeamsODB.HasValue)
            parameters.Add("EnableATPForSPOTeamsODB", updateDto.EnableATPForSPOTeamsODB.Value);

        if (updateDto.EnableSafeDocs.HasValue)
            parameters.Add("EnableSafeDocs", updateDto.EnableSafeDocs.Value);

        if (updateDto.AllowSafeDocsOpen.HasValue)
            parameters.Add("AllowSafeDocsOpen", updateDto.AllowSafeDocsOpen.Value);

        return parameters;
    }
}
