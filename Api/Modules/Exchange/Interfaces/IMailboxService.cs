using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;

namespace CIPP.Api.Modules.Exchange.Interfaces;

public interface IMailboxService {
    Task<List<MailboxDto>> GetMailboxesAsync(Guid tenantId, string? mailboxType = null, CancellationToken cancellationToken = default);
    Task<MailboxDetailsDto?> GetMailboxAsync(Guid tenantId, string userId, CancellationToken cancellationToken = default);
    Task UpdateMailboxAsync(Guid tenantId, string userId, UpdateMailboxDto updateDto, CancellationToken cancellationToken = default);
    Task<MailboxForwardingDto> GetMailboxForwardingAsync(Guid tenantId, string userId, CancellationToken cancellationToken = default);
    Task UpdateMailboxForwardingAsync(Guid tenantId, string userId, UpdateMailboxForwardingDto updateDto, CancellationToken cancellationToken = default);
    Task<List<MailboxPermissionDto>> GetMailboxPermissionsAsync(Guid tenantId, string userId, CancellationToken cancellationToken = default);
    Task UpdateMailboxPermissionsAsync(Guid tenantId, string userId, UpdateMailboxPermissionsDto updateDto, CancellationToken cancellationToken = default);
    Task<PagedResponse<InboxRuleDto>> GetInboxRulesAsync(Guid tenantId, string mailboxId, PagingParameters pagingParams, CancellationToken cancellationToken = default);
    Task<InboxRuleDto?> GetInboxRuleAsync(Guid tenantId, string mailboxId, string ruleId, CancellationToken cancellationToken = default);
    Task<string> CreateInboxRuleAsync(Guid tenantId, string mailboxId, CreateInboxRuleDto createDto, CancellationToken cancellationToken = default);
    Task UpdateInboxRuleAsync(Guid tenantId, string mailboxId, string ruleId, UpdateInboxRuleDto updateDto, CancellationToken cancellationToken = default);
    Task DeleteInboxRuleAsync(Guid tenantId, string mailboxId, string ruleId, CancellationToken cancellationToken = default);
    Task EnableArchiveAsync(Guid tenantId, string mailboxId, CancellationToken cancellationToken = default);
    Task<MailboxStatisticsDto?> GetMailboxStatisticsAsync(Guid tenantId, string mailboxId, CancellationToken cancellationToken = default);
    Task<MailboxQuotaDto?> GetMailboxQuotaAsync(Guid tenantId, string mailboxId, CancellationToken cancellationToken = default);
    Task UpdateMailboxQuotaAsync(Guid tenantId, string mailboxId, UpdateMailboxQuotaDto updateDto, CancellationToken cancellationToken = default);
    Task UpdateLitigationHoldAsync(Guid tenantId, string mailboxId, LitigationHoldDto holdDto, CancellationToken cancellationToken = default);
}
