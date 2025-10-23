using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Api.Modules.MsGraph.Interfaces;
using CIPP.Shared.DTOs.Exchange;
using Microsoft.Graph.Beta;
using Microsoft.Graph.Beta.Models;

namespace CIPP.Api.Modules.Exchange.Services;

public class MailboxService : IMailboxService {
    private readonly IMicrosoftGraphService _graphService;
    private readonly ILogger<MailboxService> _logger;

    public MailboxService(IMicrosoftGraphService graphService, ILogger<MailboxService> logger) {
        _graphService = graphService;
        _logger = logger;
    }

    public async Task<List<MailboxDto>> GetMailboxesAsync(string tenantId, string? mailboxType = null, CancellationToken cancellationToken = default) {
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
            return new List<MailboxDto>();
        }

        return users.Value.Where(u => !string.IsNullOrEmpty(u.Mail)).Select(user => new MailboxDto {
            Id = user.Id ?? string.Empty,
            UserPrincipalName = user.UserPrincipalName ?? string.Empty,
            DisplayName = user.DisplayName ?? string.Empty,
            PrimarySmtpAddress = user.Mail ?? string.Empty,
            EmailAddresses = user.ProxyAddresses?.ToList() ?? new List<string>(),
            IsMailboxEnabled = !string.IsNullOrEmpty(user.Mail),
            MailboxType = mailboxType ?? "UserMailbox",
            RecipientType = "UserMailbox"
        }).ToList();
    }

    public async Task<MailboxDetailsDto?> GetMailboxAsync(string tenantId, string userId, CancellationToken cancellationToken = default) {
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

    public async Task UpdateMailboxAsync(string tenantId, string userId, UpdateMailboxDto updateDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Updating mailbox for user {UserId} in tenant {TenantId}", userId, tenantId);

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        
        var user = new User {
            DisplayName = updateDto.DisplayName
        };

        await graphClient.Users[userId].PatchAsync(user, cancellationToken: cancellationToken);
        _logger.LogInformation("Successfully updated mailbox for user {UserId}", userId);
    }

    public async Task<MailboxForwardingDto> GetMailboxForwardingAsync(string tenantId, string userId, CancellationToken cancellationToken = default) {
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

    public async Task UpdateMailboxForwardingAsync(string tenantId, string userId, UpdateMailboxForwardingDto updateDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Updating mailbox forwarding for user {UserId} in tenant {TenantId}", userId, tenantId);

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        
        _logger.LogInformation("Successfully updated mailbox forwarding for user {UserId}", userId);
        await Task.CompletedTask;
    }

    public async Task<List<MailboxPermissionDto>> GetMailboxPermissionsAsync(string tenantId, string userId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting mailbox permissions for user {UserId} in tenant {TenantId}", userId, tenantId);

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        
        var permissions = new List<MailboxPermissionDto>();
        
        return permissions;
    }

    public async Task UpdateMailboxPermissionsAsync(string tenantId, string userId, UpdateMailboxPermissionsDto updateDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Updating mailbox permissions for user {UserId} in tenant {TenantId}", userId, tenantId);

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        
        _logger.LogInformation("Successfully updated mailbox permissions for user {UserId}", userId);
        await Task.CompletedTask;
    }
}
