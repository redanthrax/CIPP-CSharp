using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Shared.DTOs.Exchange;

namespace CIPP.Api.Modules.Exchange.Services;

public class MailboxDelegateService : IMailboxDelegateService {
    private readonly IExchangeOnlineService _exoService;
    private readonly ILogger<MailboxDelegateService> _logger;

    public MailboxDelegateService(IExchangeOnlineService exoService, ILogger<MailboxDelegateService> logger) {
        _exoService = exoService;
        _logger = logger;
    }

    public async Task<List<MailboxDelegateDto>> GetMailboxDelegatesAsync(Guid tenantId, string mailboxId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting mailbox delegates for {MailboxId} in tenant {TenantId}", mailboxId, tenantId);
        
        var delegates = new List<MailboxDelegateDto>();
        
        // Get Full Access permissions
        var fullAccessParams = new Dictionary<string, object> {
            { "Identity", mailboxId }
        };
        
        var fullAccessPermissions = await _exoService.ExecuteCmdletListAsync<dynamic>(
            tenantId,
            "Get-MailboxPermission",
            fullAccessParams,
            cancellationToken
        );

        foreach (var permission in fullAccessPermissions) {
            if (permission.User != null && !permission.User.ToString().StartsWith("NT AUTHORITY") && 
                !permission.User.ToString().StartsWith("S-1-5") && permission.AccessRights != null) {
                
                var accessRights = permission.AccessRights.ToString().Split(',');
                bool hasFullAccess = false;
                foreach (var right in accessRights) {
                    if (right.Trim() == "FullAccess") {
                        hasFullAccess = true;
                        break;
                    }
                }
                if (hasFullAccess) {
                    delegates.Add(new MailboxDelegateDto {
                        MailboxId = mailboxId,
                        DelegateUser = permission.User.ToString(),
                        Permissions = new List<string> { "FullAccess" },
                        SendOnBehalfOf = false,
                        TenantId = tenantId
                    });
                }
            }
        }

        // Get SendAs permissions
        var sendAsParams = new Dictionary<string, object> {
            { "Identity", mailboxId }
        };
        
        var sendAsPermissions = await _exoService.ExecuteCmdletListAsync<dynamic>(
            tenantId,
            "Get-RecipientPermission",
            sendAsParams,
            cancellationToken
        );

        foreach (var permission in sendAsPermissions) {
            if (permission.Trustee != null && permission.AccessRights != null) {
                var accessRights = permission.AccessRights.ToString().Split(',');
                bool hasSendAs = false;
                foreach (var right in accessRights) {
                    if (right.Trim() == "SendAs") {
                        hasSendAs = true;
                        break;
                    }
                }
                if (hasSendAs) {
                    var existing = delegates.FirstOrDefault(d => d.DelegateUser == permission.Trustee.ToString());
                    if (existing != null) {
                        existing.Permissions.Add("SendAs");
                    } else {
                        delegates.Add(new MailboxDelegateDto {
                            MailboxId = mailboxId,
                            DelegateUser = permission.Trustee.ToString(),
                            Permissions = new List<string> { "SendAs" },
                            SendOnBehalfOf = false,
                            TenantId = tenantId
                        });
                    }
                }
            }
        }

        // Get SendOnBehalfOf permissions
        var mailboxParams = new Dictionary<string, object> {
            { "Identity", mailboxId }
        };
        
        var mailboxData = await _exoService.ExecuteCmdletAsync<dynamic>(
            tenantId,
            "Get-Mailbox",
            mailboxParams,
            cancellationToken
        );

        if (mailboxData?.GrantSendOnBehalfTo != null) {
            var grantees = mailboxData.GrantSendOnBehalfTo.ToString().Split(',');
            foreach (var grantee in grantees) {
                if (!string.IsNullOrWhiteSpace(grantee)) {
                    var cleanGrantee = grantee.Trim();
                    var existing = delegates.FirstOrDefault(d => d.DelegateUser == cleanGrantee);
                    if (existing != null) {
                        existing.SendOnBehalfOf = true;
                    } else {
                        delegates.Add(new MailboxDelegateDto {
                            MailboxId = mailboxId,
                            DelegateUser = cleanGrantee,
                            Permissions = new List<string>(),
                            SendOnBehalfOf = true,
                            TenantId = tenantId
                        });
                    }
                }
            }
        }

        return delegates;
    }

    public async Task AddMailboxDelegateAsync(Guid tenantId, string mailboxId, AddMailboxDelegateDto delegateDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Adding delegate {Delegate} to mailbox {MailboxId} in tenant {TenantId}", 
            delegateDto.DelegateUser, mailboxId, tenantId);

        // Add FullAccess permission
        if (delegateDto.Permissions.Contains("FullAccess")) {
            var fullAccessParams = new Dictionary<string, object> {
                { "Identity", mailboxId },
                { "User", delegateDto.DelegateUser },
                { "AccessRights", "FullAccess" },
                { "InheritanceType", "All" },
                { "AutoMapping", delegateDto.AutoMapping },
                { "Confirm", false }
            };

            await _exoService.ExecuteCmdletNoResultAsync(
                tenantId,
                "Add-MailboxPermission",
                fullAccessParams,
                cancellationToken
            );
        }

        // Add SendAs permission
        if (delegateDto.Permissions.Contains("SendAs")) {
            var sendAsParams = new Dictionary<string, object> {
                { "Identity", mailboxId },
                { "Trustee", delegateDto.DelegateUser },
                { "AccessRights", "SendAs" },
                { "Confirm", false }
            };

            await _exoService.ExecuteCmdletNoResultAsync(
                tenantId,
                "Add-RecipientPermission",
                sendAsParams,
                cancellationToken
            );
        }

        // Add SendOnBehalfOf permission
        if (delegateDto.SendOnBehalfOf) {
            var sendOnBehalfParams = new Dictionary<string, object> {
                { "Identity", mailboxId },
                { "GrantSendOnBehalfTo", new Dictionary<string, object> {
                    { "@odata.type", "microsoft.graph.Collection" },
                    { "Add", delegateDto.DelegateUser }
                }},
                { "Confirm", false }
            };

            await _exoService.ExecuteCmdletNoResultAsync(
                tenantId,
                "Set-Mailbox",
                sendOnBehalfParams,
                cancellationToken
            );
        }

        _logger.LogInformation("Successfully added delegate {Delegate} to mailbox {MailboxId}", 
            delegateDto.DelegateUser, mailboxId);
    }

    public async Task RemoveMailboxDelegateAsync(Guid tenantId, string mailboxId, RemoveMailboxDelegateDto delegateDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Removing delegate {Delegate} from mailbox {MailboxId} in tenant {TenantId}", 
            delegateDto.DelegateUser, mailboxId, tenantId);

        // Remove FullAccess permission
        if (delegateDto.Permissions.Contains("FullAccess")) {
            var fullAccessParams = new Dictionary<string, object> {
                { "Identity", mailboxId },
                { "User", delegateDto.DelegateUser },
                { "AccessRights", "FullAccess" },
                { "InheritanceType", "All" },
                { "Confirm", false }
            };

            await _exoService.ExecuteCmdletNoResultAsync(
                tenantId,
                "Remove-MailboxPermission",
                fullAccessParams,
                cancellationToken
            );
        }

        // Remove SendAs permission
        if (delegateDto.Permissions.Contains("SendAs")) {
            var sendAsParams = new Dictionary<string, object> {
                { "Identity", mailboxId },
                { "Trustee", delegateDto.DelegateUser },
                { "AccessRights", "SendAs" },
                { "Confirm", false }
            };

            await _exoService.ExecuteCmdletNoResultAsync(
                tenantId,
                "Remove-RecipientPermission",
                sendAsParams,
                cancellationToken
            );
        }

        // Remove SendOnBehalfOf permission
        if (delegateDto.RemoveSendOnBehalfOf) {
            var sendOnBehalfParams = new Dictionary<string, object> {
                { "Identity", mailboxId },
                { "GrantSendOnBehalfTo", new Dictionary<string, object> {
                    { "@odata.type", "microsoft.graph.Collection" },
                    { "Remove", delegateDto.DelegateUser }
                }},
                { "Confirm", false }
            };

            await _exoService.ExecuteCmdletNoResultAsync(
                tenantId,
                "Set-Mailbox",
                sendOnBehalfParams,
                cancellationToken
            );
        }

        _logger.LogInformation("Successfully removed delegate {Delegate} from mailbox {MailboxId}", 
            delegateDto.DelegateUser, mailboxId);
    }
}
