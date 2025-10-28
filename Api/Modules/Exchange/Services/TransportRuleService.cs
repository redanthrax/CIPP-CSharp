using CIPP.Api.Extensions;
using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;

namespace CIPP.Api.Modules.Exchange.Services;

public class TransportRuleService : ITransportRuleService {
    private readonly IExchangeOnlineService _exoService;
    private readonly ILogger<TransportRuleService> _logger;

    public TransportRuleService(IExchangeOnlineService exoService, ILogger<TransportRuleService> logger) {
        _exoService = exoService;
        _logger = logger;
    }

    public async Task<PagedResponse<TransportRuleDto>> GetTransportRulesAsync(Guid tenantId, PagingParameters pagingParams, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting transport rules for tenant {TenantId}", tenantId);
        
        var rules = await _exoService.ExecuteCmdletListAsync<TransportRuleDto>(
            tenantId,
            "Get-TransportRule",
            null,
            cancellationToken
        );

        return rules.ToPagedResponse(pagingParams);
    }

    public async Task<TransportRuleDetailsDto?> GetTransportRuleAsync(Guid tenantId, string ruleId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting transport rule {RuleId} for tenant {TenantId}", ruleId, tenantId);
        
        var parameters = new Dictionary<string, object> {
            { "Identity", ruleId }
        };

        var rule = await _exoService.ExecuteCmdletAsync<TransportRuleDetailsDto>(
            tenantId,
            "Get-TransportRule",
            parameters,
            cancellationToken
        );

        return rule;
    }

    public async Task<string> CreateTransportRuleAsync(Guid tenantId, CreateTransportRuleDto createDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Creating transport rule {RuleName} for tenant {TenantId}", createDto.Name, tenantId);
        
        var parameters = BuildCreateParameters(createDto);

        await _exoService.ExecuteCmdletNoResultAsync(
            tenantId,
            "New-TransportRule",
            parameters,
            cancellationToken
        );

        _logger.LogInformation("Successfully created transport rule {RuleName}", createDto.Name);
        return createDto.Name;
    }

    public async Task UpdateTransportRuleAsync(Guid tenantId, string ruleId, UpdateTransportRuleDto updateDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Updating transport rule {RuleId} for tenant {TenantId}", ruleId, tenantId);
        
        var parameters = BuildUpdateParameters(ruleId, updateDto);

        await _exoService.ExecuteCmdletNoResultAsync(
            tenantId,
            "Set-TransportRule",
            parameters,
            cancellationToken
        );

        _logger.LogInformation("Successfully updated transport rule {RuleId}", ruleId);
    }

    public async Task DeleteTransportRuleAsync(Guid tenantId, string ruleId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Deleting transport rule {RuleId} for tenant {TenantId}", ruleId, tenantId);
        
        var parameters = new Dictionary<string, object> {
            { "Identity", ruleId },
            { "Confirm", false }
        };

        await _exoService.ExecuteCmdletNoResultAsync(
            tenantId,
            "Remove-TransportRule",
            parameters,
            cancellationToken
        );

        _logger.LogInformation("Successfully deleted transport rule {RuleId}", ruleId);
    }

    public async Task EnableTransportRuleAsync(Guid tenantId, string ruleId, bool enable, CancellationToken cancellationToken = default) {
        _logger.LogInformation("{Action} transport rule {RuleId} for tenant {TenantId}", 
            enable ? "Enabling" : "Disabling", ruleId, tenantId);
        
        var cmdlet = enable ? "Enable-TransportRule" : "Disable-TransportRule";
        var parameters = new Dictionary<string, object> {
            { "Identity", ruleId },
            { "Confirm", false }
        };

        await _exoService.ExecuteCmdletNoResultAsync(
            tenantId,
            cmdlet,
            parameters,
            cancellationToken
        );

        _logger.LogInformation("Successfully {Action} transport rule {RuleId}", 
            enable ? "enabled" : "disabled", ruleId);
    }

    private Dictionary<string, object> BuildCreateParameters(CreateTransportRuleDto createDto) {
        var parameters = new Dictionary<string, object> {
            { "Name", createDto.Name }
        };

        if (!string.IsNullOrEmpty(createDto.Description))
            parameters.Add("Description", createDto.Description);

        if (createDto.Priority > 0)
            parameters.Add("Priority", createDto.Priority);

        parameters.Add("Enabled", createDto.Enabled);

        if (!string.IsNullOrEmpty(createDto.Mode))
            parameters.Add("Mode", createDto.Mode);

        if (!string.IsNullOrEmpty(createDto.Comments))
            parameters.Add("Comments", createDto.Comments);

        if (createDto.ActivationDate.HasValue)
            parameters.Add("ActivationDate", createDto.ActivationDate.Value);

        if (createDto.ExpiryDate.HasValue)
            parameters.Add("ExpiryDate", createDto.ExpiryDate.Value);

        AddConditionParameters(parameters, createDto.Conditions);
        AddActionParameters(parameters, createDto.Actions);

        return parameters;
    }

    private Dictionary<string, object> BuildUpdateParameters(string ruleId, UpdateTransportRuleDto updateDto) {
        var parameters = new Dictionary<string, object> {
            { "Identity", ruleId }
        };

        if (!string.IsNullOrEmpty(updateDto.Name))
            parameters.Add("Name", updateDto.Name);

        if (!string.IsNullOrEmpty(updateDto.Description))
            parameters.Add("Description", updateDto.Description);

        if (updateDto.Priority.HasValue)
            parameters.Add("Priority", updateDto.Priority.Value);

        if (updateDto.Enabled.HasValue)
            parameters.Add("Enabled", updateDto.Enabled.Value);

        if (!string.IsNullOrEmpty(updateDto.Mode))
            parameters.Add("Mode", updateDto.Mode);

        if (!string.IsNullOrEmpty(updateDto.Comments))
            parameters.Add("Comments", updateDto.Comments);

        if (updateDto.ActivationDate.HasValue)
            parameters.Add("ActivationDate", updateDto.ActivationDate.Value);

        if (updateDto.ExpiryDate.HasValue)
            parameters.Add("ExpiryDate", updateDto.ExpiryDate.Value);

        if (updateDto.Conditions != null)
            AddConditionParameters(parameters, updateDto.Conditions);

        if (updateDto.Actions != null)
            AddActionParameters(parameters, updateDto.Actions);

        return parameters;
    }

    private void AddConditionParameters(Dictionary<string, object> parameters, TransportRuleConditionsDto conditions) {
        if (conditions.From?.Any() == true)
            parameters.Add("From", conditions.From.ToArray());

        if (conditions.FromMemberOf?.Any() == true)
            parameters.Add("FromMemberOf", conditions.FromMemberOf.ToArray());

        if (conditions.SentTo?.Any() == true)
            parameters.Add("SentTo", conditions.SentTo.ToArray());

        if (conditions.SentToMemberOf?.Any() == true)
            parameters.Add("SentToMemberOf", conditions.SentToMemberOf.ToArray());

        if (conditions.SubjectContainsWords?.Any() == true)
            parameters.Add("SubjectContainsWords", conditions.SubjectContainsWords.ToArray());

        if (conditions.SubjectMatchesPatterns?.Any() == true)
            parameters.Add("SubjectMatchesPatterns", conditions.SubjectMatchesPatterns.ToArray());

        if (conditions.FromAddressContainsWords?.Any() == true)
            parameters.Add("FromAddressContainsWords", conditions.FromAddressContainsWords.ToArray());

        if (conditions.RecipientAddressContainsWords?.Any() == true)
            parameters.Add("RecipientAddressContainsWords", conditions.RecipientAddressContainsWords.ToArray());

        if (conditions.AttachmentNameMatchesPatterns?.Any() == true)
            parameters.Add("AttachmentNameMatchesPatterns", conditions.AttachmentNameMatchesPatterns.ToArray());

        if (conditions.HeaderContainsWords?.Any() == true)
            parameters.Add("HeaderContainsWords", conditions.HeaderContainsWords.ToArray());

        if (!string.IsNullOrEmpty(conditions.MessageSizeOver))
            parameters.Add("MessageSizeOver", conditions.MessageSizeOver);

        if (!string.IsNullOrEmpty(conditions.SCLOver))
            parameters.Add("SCLOver", conditions.SCLOver);

        if (conditions.HasAttachment.HasValue)
            parameters.Add("AttachmentProcessingLimitExceeded", conditions.HasAttachment.Value);

        if (conditions.AnyOfToHeader?.Any() == true)
            parameters.Add("AnyOfToHeader", conditions.AnyOfToHeader.ToArray());

        if (conditions.AnyOfCcHeader?.Any() == true)
            parameters.Add("AnyOfCcHeader", conditions.AnyOfCcHeader.ToArray());

        if (!string.IsNullOrEmpty(conditions.SenderDomainIs))
            parameters.Add("SenderDomainIs", conditions.SenderDomainIs);

        if (!string.IsNullOrEmpty(conditions.RecipientDomainIs))
            parameters.Add("RecipientDomainIs", conditions.RecipientDomainIs);
    }

    private void AddActionParameters(Dictionary<string, object> parameters, TransportRuleActionsDto actions) {
        if (actions.AddToRecipients?.Any() == true)
            parameters.Add("AddToRecipients", actions.AddToRecipients.ToArray());

        if (actions.BlindCopyTo?.Any() == true)
            parameters.Add("BlindCopyTo", actions.BlindCopyTo.ToArray());

        if (actions.CopyTo?.Any() == true)
            parameters.Add("CopyTo", actions.CopyTo.ToArray());

        if (actions.RedirectMessageTo?.Any() == true)
            parameters.Add("RedirectMessageTo", actions.RedirectMessageTo.ToArray());

        if (!string.IsNullOrEmpty(actions.RejectMessageReasonText))
            parameters.Add("RejectMessageReasonText", actions.RejectMessageReasonText);

        if (!string.IsNullOrEmpty(actions.DeleteMessage))
            parameters.Add("DeleteMessage", actions.DeleteMessage);

        if (!string.IsNullOrEmpty(actions.Quarantine))
            parameters.Add("Quarantine", actions.Quarantine);

        if (!string.IsNullOrEmpty(actions.SetSCL))
            parameters.Add("SetSCL", actions.SetSCL);

        if (!string.IsNullOrEmpty(actions.PrependSubject))
            parameters.Add("PrependSubject", actions.PrependSubject);

        if (!string.IsNullOrEmpty(actions.SetHeaderName))
            parameters.Add("SetHeaderName", actions.SetHeaderName);

        if (!string.IsNullOrEmpty(actions.SetHeaderValue))
            parameters.Add("SetHeaderValue", actions.SetHeaderValue);

        if (!string.IsNullOrEmpty(actions.RemoveHeader))
            parameters.Add("RemoveHeader", actions.RemoveHeader);

        if (!string.IsNullOrEmpty(actions.ApplyClassification))
            parameters.Add("ApplyClassification", actions.ApplyClassification);

        if (!string.IsNullOrEmpty(actions.ApplyHtmlDisclaimerText))
            parameters.Add("ApplyHtmlDisclaimerText", actions.ApplyHtmlDisclaimerText);

        if (!string.IsNullOrEmpty(actions.ApplyHtmlDisclaimerLocation))
            parameters.Add("ApplyHtmlDisclaimerLocation", actions.ApplyHtmlDisclaimerLocation);

        if (!string.IsNullOrEmpty(actions.ModerateMessageByUser))
            parameters.Add("ModerateMessageByUser", actions.ModerateMessageByUser);

        if (actions.StopRuleProcessing.HasValue)
            parameters.Add("StopRuleProcessing", actions.StopRuleProcessing.Value);
    }
}
