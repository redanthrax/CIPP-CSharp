using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;

namespace CIPP.Api.Modules.Exchange.Interfaces;

public interface IMailboxService {
    Task<List<MailboxDto>> GetMailboxesAsync(string tenantId, string? mailboxType = null, CancellationToken cancellationToken = default);
    Task<MailboxDetailsDto?> GetMailboxAsync(string tenantId, string userId, CancellationToken cancellationToken = default);
    Task UpdateMailboxAsync(string tenantId, string userId, UpdateMailboxDto updateDto, CancellationToken cancellationToken = default);
    Task<MailboxForwardingDto> GetMailboxForwardingAsync(string tenantId, string userId, CancellationToken cancellationToken = default);
    Task UpdateMailboxForwardingAsync(string tenantId, string userId, UpdateMailboxForwardingDto updateDto, CancellationToken cancellationToken = default);
    Task<List<MailboxPermissionDto>> GetMailboxPermissionsAsync(string tenantId, string userId, CancellationToken cancellationToken = default);
    Task UpdateMailboxPermissionsAsync(string tenantId, string userId, UpdateMailboxPermissionsDto updateDto, CancellationToken cancellationToken = default);
    Task<PagedResponse<InboxRuleDto>> GetInboxRulesAsync(string tenantId, string mailboxId, PagingParameters pagingParams, CancellationToken cancellationToken = default);
    Task<InboxRuleDto?> GetInboxRuleAsync(string tenantId, string mailboxId, string ruleId, CancellationToken cancellationToken = default);
    Task<string> CreateInboxRuleAsync(string tenantId, string mailboxId, CreateInboxRuleDto createDto, CancellationToken cancellationToken = default);
    Task UpdateInboxRuleAsync(string tenantId, string mailboxId, string ruleId, UpdateInboxRuleDto updateDto, CancellationToken cancellationToken = default);
    Task DeleteInboxRuleAsync(string tenantId, string mailboxId, string ruleId, CancellationToken cancellationToken = default);
    Task EnableArchiveAsync(string tenantId, string mailboxId, CancellationToken cancellationToken = default);
    Task<MailboxStatisticsDto?> GetMailboxStatisticsAsync(string tenantId, string mailboxId, CancellationToken cancellationToken = default);
    Task<MailboxQuotaDto?> GetMailboxQuotaAsync(string tenantId, string mailboxId, CancellationToken cancellationToken = default);
    Task UpdateMailboxQuotaAsync(string tenantId, string mailboxId, UpdateMailboxQuotaDto updateDto, CancellationToken cancellationToken = default);
    Task UpdateLitigationHoldAsync(string tenantId, string mailboxId, LitigationHoldDto holdDto, CancellationToken cancellationToken = default);
}
