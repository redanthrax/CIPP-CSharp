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
}
