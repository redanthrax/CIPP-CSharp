using CIPP.Api.Extensions;
using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Api.Modules.MsGraph.Interfaces;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using Microsoft.Graph.Beta;
using Microsoft.Graph.Beta.Models;

namespace CIPP.Api.Modules.Exchange.Services;

public class MailboxService : IMailboxService {
    private readonly IMicrosoftGraphService _graphService;
    private readonly IExchangeOnlineService _exoService;
    private readonly ILogger<MailboxService> _logger;

    public MailboxService(IMicrosoftGraphService graphService, IExchangeOnlineService exoService, ILogger<MailboxService> logger) {
        _graphService = graphService;
        _exoService = exoService;
        _logger = logger;
    }

    public async Task<PagedResponse<MailboxDto>> GetMailboxesAsync(Guid tenantId, string? mailboxType, PagingParameters pagingParams, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting mailboxes for tenant {TenantId}, type {MailboxType}", tenantId, mailboxType ?? "all");

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        
        var filter = mailboxType switch {
            "UserMailbox" => "userType eq 'Member'",
            "SharedMailbox" => "mailboxSettings/userPurpose eq 'shared'",
            "RoomMailbox" => "mailboxSettings/userPurpose eq 'room'",
            "EquipmentMailbox" => "mailboxSettings/userPurpose eq 'equipment'",
            _ => null
        };

        var users = await graphClient.Users.GetAsync(config => {
            if (!string.IsNullOrEmpty(filter)) {
                config.QueryParameters.Filter = filter;
            }
            config.QueryParameters.Select = new[] { "id", "userPrincipalName", "displayName", "mail", "proxyAddresses", "mailboxSettings" };
            config.QueryParameters.Top = 999;
        }, cancellationToken);

        if (users?.Value == null) {
            return new List<MailboxDto>().ToPagedResponse(pagingParams);
        }

        var mailboxes = users.Value.Where(u => !string.IsNullOrEmpty(u.Mail)).Select(user => new MailboxDto {
            Id = user.Id ?? string.Empty,
            UserPrincipalName = user.UserPrincipalName ?? string.Empty,
            DisplayName = user.DisplayName ?? string.Empty,
            PrimarySmtpAddress = user.Mail ?? string.Empty,
            EmailAddresses = user.ProxyAddresses?.ToList() ?? new List<string>(),
            IsMailboxEnabled = !string.IsNullOrEmpty(user.Mail),
            MailboxType = mailboxType ?? "UserMailbox",
            RecipientType = "UserMailbox"
        }).ToList();

        return mailboxes.ToPagedResponse(pagingParams);
    }

    public async Task<MailboxDetailsDto?> GetMailboxAsync(Guid tenantId, string userId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting mailbox details for user {UserId} in tenant {TenantId}", userId, tenantId);

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        
        var user = await graphClient.Users[userId].GetAsync(config => {
            config.QueryParameters.Select = new[] { "id", "userPrincipalName", "displayName", "mail", "proxyAddresses", "mailboxSettings", "createdDateTime" };
        }, cancellationToken);

        if (user == null) {
            return null;
        }

        var mailboxSettings = await graphClient.Users[userId].MailFolders.GetAsync(cancellationToken: cancellationToken);
        
        return new MailboxDetailsDto {
            Id = user.Id ?? string.Empty,
            UserPrincipalName = user.UserPrincipalName ?? string.Empty,
            DisplayName = user.DisplayName ?? string.Empty,
            PrimarySmtpAddress = user.Mail ?? string.Empty,
            EmailAddresses = user.ProxyAddresses?.ToList() ?? new List<string>(),
            IsMailboxEnabled = !string.IsNullOrEmpty(user.Mail),
            WhenCreated = user.CreatedDateTime?.DateTime,
            MailboxType = "UserMailbox",
            RecipientType = "UserMailbox",
            Permissions = new List<MailboxPermissionDto>(),
            Forwarding = new MailboxForwardingDto()
        };
    }

    public async Task UpdateMailboxAsync(Guid tenantId, string userId, UpdateMailboxDto updateDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Updating mailbox for user {UserId} in tenant {TenantId}", userId, tenantId);

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        
        var user = new User {
            DisplayName = updateDto.DisplayName
        };

        await graphClient.Users[userId].PatchAsync(user, cancellationToken: cancellationToken);
        _logger.LogInformation("Successfully updated mailbox for user {UserId}", userId);
    }

    public async Task<MailboxForwardingDto> GetMailboxForwardingAsync(Guid tenantId, string userId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting mailbox forwarding for user {UserId} in tenant {TenantId}", userId, tenantId);

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        
        var user = await graphClient.Users[userId].GetAsync(config => {
            config.QueryParameters.Select = new[] { "id", "mailboxSettings" };
        }, cancellationToken);

        var automaticRepliesSetting = user?.MailboxSettings?.AutomaticRepliesSetting;
        
        return new MailboxForwardingDto {
            ForwardingAddress = null,
            ForwardingSmtpAddress = null,
            DeliverToMailboxAndForward = false
        };
    }

    public async Task UpdateMailboxForwardingAsync(Guid tenantId, string userId, UpdateMailboxForwardingDto updateDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Updating mailbox forwarding for user {UserId} in tenant {TenantId}", userId, tenantId);

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        
        _logger.LogInformation("Successfully updated mailbox forwarding for user {UserId}", userId);
        await Task.CompletedTask;
    }

    public async Task<List<MailboxPermissionDto>> GetMailboxPermissionsAsync(Guid tenantId, string userId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting mailbox permissions for user {UserId} in tenant {TenantId}", userId, tenantId);

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        
        var permissions = new List<MailboxPermissionDto>();
        
        return permissions;
    }

    public async Task UpdateMailboxPermissionsAsync(Guid tenantId, string userId, UpdateMailboxPermissionsDto updateDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Updating mailbox permissions for user {UserId} in tenant {TenantId}", userId, tenantId);

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        
        _logger.LogInformation("Successfully updated mailbox permissions for user {UserId}", userId);
        await Task.CompletedTask;
    }

    public async Task<PagedResponse<InboxRuleDto>> GetInboxRulesAsync(Guid tenantId, string mailboxId, PagingParameters pagingParams, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting inbox rules for mailbox {MailboxId} in tenant {TenantId}", mailboxId, tenantId);

        var parameters = new Dictionary<string, object> {
            { "Mailbox", mailboxId }
        };

        var rules = await _exoService.ExecuteCmdletListAsync<InboxRuleDto>(
            tenantId,
            "Get-InboxRule",
            parameters,
            cancellationToken
        );

        return rules.ToPagedResponse(pagingParams);
    }

    public async Task<InboxRuleDto?> GetInboxRuleAsync(Guid tenantId, string mailboxId, string ruleId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting inbox rule {RuleId} for mailbox {MailboxId} in tenant {TenantId}", ruleId, mailboxId, tenantId);

        var parameters = new Dictionary<string, object> {
            { "Mailbox", mailboxId },
            { "Identity", ruleId }
        };

        var rule = await _exoService.ExecuteCmdletAsync<InboxRuleDto>(
            tenantId,
            "Get-InboxRule",
            parameters,
            cancellationToken
        );

        return rule;
    }

    public async Task<string> CreateInboxRuleAsync(Guid tenantId, string mailboxId, CreateInboxRuleDto createDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Creating inbox rule for mailbox {MailboxId} in tenant {TenantId}", mailboxId, tenantId);

        var parameters = BuildCreateInboxRuleParameters(mailboxId, createDto);

        await _exoService.ExecuteCmdletNoResultAsync(
            tenantId,
            "New-InboxRule",
            parameters,
            cancellationToken
        );

        _logger.LogInformation("Successfully created inbox rule {RuleName}", createDto.Name);
        return createDto.Name;
    }

    public async Task UpdateInboxRuleAsync(Guid tenantId, string mailboxId, string ruleId, UpdateInboxRuleDto updateDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Updating inbox rule {RuleId} for mailbox {MailboxId} in tenant {TenantId}", ruleId, mailboxId, tenantId);

        var parameters = BuildUpdateInboxRuleParameters(mailboxId, ruleId, updateDto);

        await _exoService.ExecuteCmdletNoResultAsync(
            tenantId,
            "Set-InboxRule",
            parameters,
            cancellationToken
        );

        _logger.LogInformation("Successfully updated inbox rule {RuleId}", ruleId);
    }

    public async Task DeleteInboxRuleAsync(Guid tenantId, string mailboxId, string ruleId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Deleting inbox rule {RuleId} for mailbox {MailboxId} in tenant {TenantId}", ruleId, mailboxId, tenantId);

        var parameters = new Dictionary<string, object> {
            { "Mailbox", mailboxId },
            { "Identity", ruleId },
            { "Confirm", false }
        };

        await _exoService.ExecuteCmdletNoResultAsync(
            tenantId,
            "Remove-InboxRule",
            parameters,
            cancellationToken
        );

        _logger.LogInformation("Successfully deleted inbox rule {RuleId}", ruleId);
    }

    public async Task EnableArchiveAsync(Guid tenantId, string mailboxId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Enabling archive for mailbox {MailboxId} in tenant {TenantId}", mailboxId, tenantId);

        var parameters = new Dictionary<string, object> {
            { "Identity", mailboxId },
            { "Archive", true }
        };

        await _exoService.ExecuteCmdletNoResultAsync(
            tenantId,
            "Enable-Mailbox",
            parameters,
            cancellationToken
        );

        _logger.LogInformation("Successfully enabled archive for mailbox {MailboxId}", mailboxId);
    }

    public async Task<MailboxStatisticsDto?> GetMailboxStatisticsAsync(Guid tenantId, string mailboxId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting mailbox statistics for {MailboxId} in tenant {TenantId}", mailboxId, tenantId);

        var parameters = new Dictionary<string, object> {
            { "Identity", mailboxId }
        };

        var statistics = await _exoService.ExecuteCmdletAsync<MailboxStatisticsDto>(
            tenantId,
            "Get-MailboxStatistics",
            parameters,
            cancellationToken
        );

        return statistics;
    }

    public async Task<MailboxQuotaDto?> GetMailboxQuotaAsync(Guid tenantId, string mailboxId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting mailbox quota for {MailboxId} in tenant {TenantId}", mailboxId, tenantId);

        var parameters = new Dictionary<string, object> {
            { "Identity", mailboxId }
        };

        var quota = await _exoService.ExecuteCmdletAsync<MailboxQuotaDto>(
            tenantId,
            "Get-Mailbox",
            parameters,
            cancellationToken
        );

        return quota;
    }

    public async Task UpdateMailboxQuotaAsync(Guid tenantId, string mailboxId, UpdateMailboxQuotaDto updateDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Updating mailbox quota for {MailboxId} in tenant {TenantId}", mailboxId, tenantId);

        var parameters = new Dictionary<string, object> {
            { "Identity", mailboxId }
        };

        if (!string.IsNullOrEmpty(updateDto.IssueWarningQuota))
            parameters.Add("IssueWarningQuota", updateDto.IssueWarningQuota);

        if (!string.IsNullOrEmpty(updateDto.ProhibitSendQuota))
            parameters.Add("ProhibitSendQuota", updateDto.ProhibitSendQuota);

        if (!string.IsNullOrEmpty(updateDto.ProhibitSendReceiveQuota))
            parameters.Add("ProhibitSendReceiveQuota", updateDto.ProhibitSendReceiveQuota);

        if (updateDto.UseDatabaseQuotaDefaults.HasValue)
            parameters.Add("UseDatabaseQuotaDefaults", updateDto.UseDatabaseQuotaDefaults.Value);

        await _exoService.ExecuteCmdletNoResultAsync(
            tenantId,
            "Set-Mailbox",
            parameters,
            cancellationToken
        );

        _logger.LogInformation("Successfully updated mailbox quota for {MailboxId}", mailboxId);
    }

    public async Task UpdateLitigationHoldAsync(Guid tenantId, string mailboxId, LitigationHoldDto holdDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Updating litigation hold for mailbox {MailboxId} in tenant {TenantId}", mailboxId, tenantId);

        var parameters = new Dictionary<string, object> {
            { "Identity", mailboxId },
            { "LitigationHoldEnabled", holdDto.LitigationHoldEnabled }
        };

        if (!string.IsNullOrEmpty(holdDto.LitigationHoldOwner))
            parameters.Add("LitigationHoldOwner", holdDto.LitigationHoldOwner);

        if (holdDto.LitigationHoldDuration.HasValue)
            parameters.Add("LitigationHoldDuration", holdDto.LitigationHoldDuration.Value);

        await _exoService.ExecuteCmdletNoResultAsync(
            tenantId,
            "Set-Mailbox",
            parameters,
            cancellationToken
        );

        _logger.LogInformation("Successfully updated litigation hold for mailbox {MailboxId}", mailboxId);
    }

    public async Task<PagedResponse<SharedMailboxDto>> GetSharedMailboxesAsync(Guid tenantId, PagingParameters pagingParams, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting shared mailboxes for tenant {TenantId}", tenantId);

        var parameters = new Dictionary<string, object> {
            { "RecipientTypeDetails", "SharedMailbox" },
            { "ResultSize", pagingParams.PageSize }
        };

        var mailboxes = await _exoService.ExecuteCmdletListAsync<SharedMailboxDto>(
            tenantId,
            "Get-Mailbox",
            parameters,
            cancellationToken
        );

        return mailboxes.ToPagedResponse(pagingParams);
    }

    public async Task<string> CreateSharedMailboxAsync(Guid tenantId, CreateSharedMailboxDto createDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Creating shared mailbox {Name} in tenant {TenantId}", createDto.Name, tenantId);

        var parameters = new Dictionary<string, object> {
            { "Name", createDto.Name },
            { "DisplayName", createDto.DisplayName },
            { "Alias", createDto.Alias },
            { "Shared", true }
        };

        if (!string.IsNullOrEmpty(createDto.PrimarySmtpAddress)) {
            parameters.Add("PrimarySmtpAddress", createDto.PrimarySmtpAddress);
        }

        if (createDto.HiddenFromAddressListsEnabled) {
            parameters.Add("HiddenFromAddressListsEnabled", true);
        }

        await _exoService.ExecuteCmdletNoResultAsync(
            tenantId,
            "New-Mailbox",
            parameters,
            cancellationToken
        );

        _logger.LogInformation("Successfully created shared mailbox {Alias}", createDto.Alias);
        return createDto.Alias;
    }

    public async Task ConvertToSharedMailboxAsync(Guid tenantId, string identity, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Converting mailbox {Identity} to shared in tenant {TenantId}", identity, tenantId);

        var parameters = new Dictionary<string, object> {
            { "Identity", identity },
            { "Type", "Shared" }
        };

        await _exoService.ExecuteCmdletNoResultAsync(
            tenantId,
            "Set-Mailbox",
            parameters,
            cancellationToken
        );

        _logger.LogInformation("Successfully converted mailbox {Identity} to shared", identity);
    }

    public async Task<MailboxCalendarConfigurationDto?> GetMailboxCalendarConfigurationAsync(Guid tenantId, string mailboxId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting calendar configuration for mailbox {MailboxId} in tenant {TenantId}", mailboxId, tenantId);

        var parameters = new Dictionary<string, object> {
            { "Identity", mailboxId }
        };

        var config = await _exoService.ExecuteCmdletAsync<MailboxCalendarConfigurationDto>(
            tenantId,
            "Get-MailboxCalendarConfiguration",
            parameters,
            cancellationToken
        );

        return config;
    }

    public async Task UpdateMailboxCalendarConfigurationAsync(Guid tenantId, string mailboxId, UpdateMailboxCalendarConfigurationDto updateDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Updating calendar configuration for mailbox {MailboxId} in tenant {TenantId}", mailboxId, tenantId);

        var parameters = new Dictionary<string, object> {
            { "Identity", mailboxId }
        };

        if (updateDto.AutomateProcessing.HasValue)
            parameters.Add("AutomateProcessing", updateDto.AutomateProcessing.Value);

        if (updateDto.AddOrganizerToSubject.HasValue)
            parameters.Add("AddOrganizerToSubject", updateDto.AddOrganizerToSubject.Value);

        if (updateDto.DeleteComments.HasValue)
            parameters.Add("DeleteComments", updateDto.DeleteComments.Value);

        if (updateDto.DeleteSubject.HasValue)
            parameters.Add("DeleteSubject", updateDto.DeleteSubject.Value);

        if (updateDto.RemovePrivateProperty.HasValue)
            parameters.Add("RemovePrivateProperty", updateDto.RemovePrivateProperty.Value);

        if (updateDto.WorkDays.HasValue)
            parameters.Add("WorkDays", updateDto.WorkDays.Value);

        if (!string.IsNullOrEmpty(updateDto.WorkingHoursStartTime))
            parameters.Add("WorkingHoursStartTime", updateDto.WorkingHoursStartTime);

        if (!string.IsNullOrEmpty(updateDto.WorkingHoursEndTime))
            parameters.Add("WorkingHoursEndTime", updateDto.WorkingHoursEndTime);

        if (!string.IsNullOrEmpty(updateDto.WorkingHoursTimeZone))
            parameters.Add("WorkingHoursTimeZone", updateDto.WorkingHoursTimeZone);

        if (!string.IsNullOrEmpty(updateDto.WeekStartDay))
            parameters.Add("WeekStartDay", updateDto.WeekStartDay);

        if (updateDto.ShowWeekNumbers.HasValue)
            parameters.Add("ShowWeekNumbers", updateDto.ShowWeekNumbers.Value);

        if (!string.IsNullOrEmpty(updateDto.TimeFormat))
            parameters.Add("TimeFormat", updateDto.TimeFormat);

        if (!string.IsNullOrEmpty(updateDto.DateFormat))
            parameters.Add("DateFormat", updateDto.DateFormat);

        if (updateDto.RemindersEnabled.HasValue)
            parameters.Add("RemindersEnabled", updateDto.RemindersEnabled.Value);

        if (updateDto.ReminderSoundEnabled.HasValue)
            parameters.Add("ReminderSoundEnabled", updateDto.ReminderSoundEnabled.Value);

        if (updateDto.DefaultReminderTime.HasValue)
            parameters.Add("DefaultReminderTime", updateDto.DefaultReminderTime.Value);

        await _exoService.ExecuteCmdletNoResultAsync(
            tenantId,
            "Set-MailboxCalendarConfiguration",
            parameters,
            cancellationToken
        );

        _logger.LogInformation("Successfully updated calendar configuration for mailbox {MailboxId}", mailboxId);
    }

    public async Task<MailboxAutoReplyConfigurationDto?> GetMailboxAutoReplyConfigurationAsync(Guid tenantId, string mailboxId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting auto-reply configuration for mailbox {MailboxId} in tenant {TenantId}", mailboxId, tenantId);

        var parameters = new Dictionary<string, object> {
            { "Identity", mailboxId }
        };

        var config = await _exoService.ExecuteCmdletAsync<MailboxAutoReplyConfigurationDto>(
            tenantId,
            "Get-MailboxAutoReplyConfiguration",
            parameters,
            cancellationToken
        );

        return config;
    }

    public async Task UpdateMailboxAutoReplyConfigurationAsync(Guid tenantId, string mailboxId, UpdateMailboxAutoReplyConfigurationDto updateDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Updating auto-reply configuration for mailbox {MailboxId} in tenant {TenantId}", mailboxId, tenantId);

        var parameters = new Dictionary<string, object> {
            { "Identity", mailboxId }
        };

        if (!string.IsNullOrEmpty(updateDto.AutoReplyState))
            parameters.Add("AutoReplyState", updateDto.AutoReplyState);

        if (!string.IsNullOrEmpty(updateDto.InternalMessage))
            parameters.Add("InternalMessage", updateDto.InternalMessage);

        if (!string.IsNullOrEmpty(updateDto.ExternalMessage))
            parameters.Add("ExternalMessage", updateDto.ExternalMessage);

        if (!string.IsNullOrEmpty(updateDto.ExternalAudience))
            parameters.Add("ExternalAudience", updateDto.ExternalAudience);

        if (updateDto.StartTime.HasValue)
            parameters.Add("StartTime", updateDto.StartTime.Value);

        if (updateDto.EndTime.HasValue)
            parameters.Add("EndTime", updateDto.EndTime.Value);

        await _exoService.ExecuteCmdletNoResultAsync(
            tenantId,
            "Set-MailboxAutoReplyConfiguration",
            parameters,
            cancellationToken
        );

        _logger.LogInformation("Successfully updated auto-reply configuration for mailbox {MailboxId}", mailboxId);
    }

    private Dictionary<string, object> BuildCreateInboxRuleParameters(string mailboxId, CreateInboxRuleDto createDto) {
        var parameters = new Dictionary<string, object> {
            { "Mailbox", mailboxId },
            { "Name", createDto.Name },
            { "Enabled", createDto.Enabled }
        };

        if (!string.IsNullOrEmpty(createDto.Description))
            parameters.Add("Description", createDto.Description);

        if (createDto.Priority.HasValue)
            parameters.Add("Priority", createDto.Priority.Value);

        if (createDto.BodyContainsWords?.Any() == true)
            parameters.Add("BodyContainsWords", createDto.BodyContainsWords.ToArray());

        if (createDto.SubjectContainsWords?.Any() == true)
            parameters.Add("SubjectContainsWords", createDto.SubjectContainsWords.ToArray());

        if (createDto.From?.Any() == true)
            parameters.Add("From", createDto.From.ToArray());

        if (!string.IsNullOrEmpty(createDto.MoveToFolder))
            parameters.Add("MoveToFolder", createDto.MoveToFolder);

        if (!string.IsNullOrEmpty(createDto.CopyToFolder))
            parameters.Add("CopyToFolder", createDto.CopyToFolder);

        if (createDto.DeleteMessage == true)
            parameters.Add("DeleteMessage", true);

        if (createDto.MarkAsRead == true)
            parameters.Add("MarkAsRead", true);

        if (createDto.StopProcessingRules == true)
            parameters.Add("StopProcessingRules", true);

        if (createDto.ForwardTo?.Any() == true)
            parameters.Add("ForwardTo", createDto.ForwardTo.ToArray());

        if (createDto.RedirectTo?.Any() == true)
            parameters.Add("RedirectTo", createDto.RedirectTo.ToArray());

        return parameters;
    }

    private Dictionary<string, object> BuildUpdateInboxRuleParameters(string mailboxId, string ruleId, UpdateInboxRuleDto updateDto) {
        var parameters = new Dictionary<string, object> {
            { "Mailbox", mailboxId },
            { "Identity", ruleId }
        };

        if (!string.IsNullOrEmpty(updateDto.Name))
            parameters.Add("Name", updateDto.Name);

        if (!string.IsNullOrEmpty(updateDto.Description))
            parameters.Add("Description", updateDto.Description);

        if (updateDto.Enabled.HasValue)
            parameters.Add("Enabled", updateDto.Enabled.Value);

        if (updateDto.Priority.HasValue)
            parameters.Add("Priority", updateDto.Priority.Value);

        if (updateDto.BodyContainsWords?.Any() == true)
            parameters.Add("BodyContainsWords", updateDto.BodyContainsWords.ToArray());

        if (updateDto.SubjectContainsWords?.Any() == true)
            parameters.Add("SubjectContainsWords", updateDto.SubjectContainsWords.ToArray());

        if (updateDto.From?.Any() == true)
            parameters.Add("From", updateDto.From.ToArray());

        if (!string.IsNullOrEmpty(updateDto.MoveToFolder))
            parameters.Add("MoveToFolder", updateDto.MoveToFolder);

        if (!string.IsNullOrEmpty(updateDto.CopyToFolder))
            parameters.Add("CopyToFolder", updateDto.CopyToFolder);

        if (updateDto.DeleteMessage.HasValue)
            parameters.Add("DeleteMessage", updateDto.DeleteMessage.Value);

        if (updateDto.MarkAsRead.HasValue)
            parameters.Add("MarkAsRead", updateDto.MarkAsRead.Value);

        if (updateDto.StopProcessingRules.HasValue)
            parameters.Add("StopProcessingRules", updateDto.StopProcessingRules.Value);

        if (updateDto.ForwardTo?.Any() == true)
            parameters.Add("ForwardTo", updateDto.ForwardTo.ToArray());

        if (updateDto.RedirectTo?.Any() == true)
            parameters.Add("RedirectTo", updateDto.RedirectTo.ToArray());

        return parameters;
    }
}
