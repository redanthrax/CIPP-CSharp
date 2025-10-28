using CIPP.Api.Extensions;
using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;

namespace CIPP.Api.Modules.Exchange.Services;

public class OwaMailboxPolicyService : IOwaMailboxPolicyService {
    private readonly IExchangeOnlineService _exoService;
    private readonly ILogger<OwaMailboxPolicyService> _logger;

    public OwaMailboxPolicyService(IExchangeOnlineService exoService, ILogger<OwaMailboxPolicyService> logger) {
        _exoService = exoService;
        _logger = logger;
    }

    public async Task<PagedResponse<OwaMailboxPolicyDto>> GetOwaMailboxPoliciesAsync(Guid tenantId, PagingParameters pagingParams, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting OWA mailbox policies for tenant {TenantId}", tenantId);
        
        var policies = await _exoService.ExecuteCmdletListAsync<OwaMailboxPolicyDto>(
            tenantId,
            "Get-OwaMailboxPolicy",
            null,
            cancellationToken
        );

        foreach (var policy in policies) {
            policy.TenantId = tenantId;
        }

        return policies.ToPagedResponse(pagingParams);
    }

    public async Task<OwaMailboxPolicyDto?> GetOwaMailboxPolicyAsync(Guid tenantId, string policyId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting OWA mailbox policy {PolicyId} for tenant {TenantId}", policyId, tenantId);
        
        var parameters = new Dictionary<string, object> {
            { "Identity", policyId }
        };

        var policy = await _exoService.ExecuteCmdletAsync<OwaMailboxPolicyDto>(
            tenantId,
            "Get-OwaMailboxPolicy",
            parameters,
            cancellationToken
        );

        if (policy != null) {
            policy.TenantId = tenantId;
        }

        return policy;
    }

    public async Task UpdateOwaMailboxPolicyAsync(Guid tenantId, string policyId, UpdateOwaMailboxPolicyDto updateDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Updating OWA mailbox policy {PolicyId} for tenant {TenantId}", policyId, tenantId);
        
        var parameters = BuildUpdateParameters(policyId, updateDto);

        await _exoService.ExecuteCmdletNoResultAsync(
            tenantId,
            "Set-OwaMailboxPolicy",
            parameters,
            cancellationToken
        );

        _logger.LogInformation("Successfully updated OWA mailbox policy {PolicyId}", policyId);
    }

    private Dictionary<string, object> BuildUpdateParameters(string policyId, UpdateOwaMailboxPolicyDto updateDto) {
        var parameters = new Dictionary<string, object> {
            { "Identity", policyId }
        };

        if (updateDto.DirectFileAccessOnPublicComputersEnabled.HasValue)
            parameters.Add("DirectFileAccessOnPublicComputersEnabled", updateDto.DirectFileAccessOnPublicComputersEnabled.Value);

        if (updateDto.DirectFileAccessOnPrivateComputersEnabled.HasValue)
            parameters.Add("DirectFileAccessOnPrivateComputersEnabled", updateDto.DirectFileAccessOnPrivateComputersEnabled.Value);

        if (updateDto.WebReadyDocumentViewingOnPublicComputersEnabled.HasValue)
            parameters.Add("WebReadyDocumentViewingOnPublicComputersEnabled", updateDto.WebReadyDocumentViewingOnPublicComputersEnabled.Value);

        if (updateDto.WebReadyDocumentViewingOnPrivateComputersEnabled.HasValue)
            parameters.Add("WebReadyDocumentViewingOnPrivateComputersEnabled", updateDto.WebReadyDocumentViewingOnPrivateComputersEnabled.Value);

        if (updateDto.ForceWebReadyDocumentViewingFirstOnPublicComputers.HasValue)
            parameters.Add("ForceWebReadyDocumentViewingFirstOnPublicComputers", updateDto.ForceWebReadyDocumentViewingFirstOnPublicComputers.Value);

        if (updateDto.ForceWebReadyDocumentViewingFirstOnPrivateComputers.HasValue)
            parameters.Add("ForceWebReadyDocumentViewingFirstOnPrivateComputers", updateDto.ForceWebReadyDocumentViewingFirstOnPrivateComputers.Value);

        if (updateDto.WacViewingOnPublicComputersEnabled.HasValue)
            parameters.Add("WacViewingOnPublicComputersEnabled", updateDto.WacViewingOnPublicComputersEnabled.Value);

        if (updateDto.WacViewingOnPrivateComputersEnabled.HasValue)
            parameters.Add("WacViewingOnPrivateComputersEnabled", updateDto.WacViewingOnPrivateComputersEnabled.Value);

        if (updateDto.ClassicAttachmentsEnabled.HasValue)
            parameters.Add("ClassicAttachmentsEnabled", updateDto.ClassicAttachmentsEnabled.Value);

        if (updateDto.AllAddressListsEnabled.HasValue)
            parameters.Add("AllAddressListsEnabled", updateDto.AllAddressListsEnabled.Value);

        if (updateDto.CalendarEnabled.HasValue)
            parameters.Add("CalendarEnabled", updateDto.CalendarEnabled.Value);

        if (updateDto.ContactsEnabled.HasValue)
            parameters.Add("ContactsEnabled", updateDto.ContactsEnabled.Value);

        if (updateDto.JournalEnabled.HasValue)
            parameters.Add("JournalEnabled", updateDto.JournalEnabled.Value);

        if (updateDto.NotesEnabled.HasValue)
            parameters.Add("NotesEnabled", updateDto.NotesEnabled.Value);

        if (updateDto.RemindersAndNotificationsEnabled.HasValue)
            parameters.Add("RemindersAndNotificationsEnabled", updateDto.RemindersAndNotificationsEnabled.Value);

        if (updateDto.SatisfactionEnabled.HasValue)
            parameters.Add("SatisfactionEnabled", updateDto.SatisfactionEnabled.Value);

        if (updateDto.TextMessagingEnabled.HasValue)
            parameters.Add("TextMessagingEnabled", updateDto.TextMessagingEnabled.Value);

        if (updateDto.ThemeSelectionEnabled.HasValue)
            parameters.Add("ThemeSelectionEnabled", updateDto.ThemeSelectionEnabled.Value);

        if (updateDto.ChangePasswordEnabled.HasValue)
            parameters.Add("ChangePasswordEnabled", updateDto.ChangePasswordEnabled.Value);

        if (updateDto.UMIntegrationEnabled.HasValue)
            parameters.Add("UMIntegrationEnabled", updateDto.UMIntegrationEnabled.Value);

        if (updateDto.WSSAccessOnPublicComputersEnabled.HasValue)
            parameters.Add("WSSAccessOnPublicComputersEnabled", updateDto.WSSAccessOnPublicComputersEnabled.Value);

        if (updateDto.WSSAccessOnPrivateComputersEnabled.HasValue)
            parameters.Add("WSSAccessOnPrivateComputersEnabled", updateDto.WSSAccessOnPrivateComputersEnabled.Value);

        if (updateDto.UNCAccessOnPublicComputersEnabled.HasValue)
            parameters.Add("UNCAccessOnPublicComputersEnabled", updateDto.UNCAccessOnPublicComputersEnabled.Value);

        if (updateDto.UNCAccessOnPrivateComputersEnabled.HasValue)
            parameters.Add("UNCAccessOnPrivateComputersEnabled", updateDto.UNCAccessOnPrivateComputersEnabled.Value);

        if (updateDto.ActiveSyncIntegrationEnabled.HasValue)
            parameters.Add("ActiveSyncIntegrationEnabled", updateDto.ActiveSyncIntegrationEnabled.Value);

        if (updateDto.AllowedFileTypes?.Any() == true)
            parameters.Add("AllowedFileTypes", updateDto.AllowedFileTypes);

        if (updateDto.AllowedMimeTypes?.Any() == true)
            parameters.Add("AllowedMimeTypes", updateDto.AllowedMimeTypes);

        if (updateDto.BlockedFileTypes?.Any() == true)
            parameters.Add("BlockedFileTypes", updateDto.BlockedFileTypes);

        if (updateDto.BlockedMimeTypes?.Any() == true)
            parameters.Add("BlockedMimeTypes", updateDto.BlockedMimeTypes);

        return parameters;
    }
}
