using CIPP.Shared.DTOs.Exchange;

namespace CIPP.Api.Modules.Exchange.Interfaces;

public interface IMailboxDelegateService {
    Task<List<MailboxDelegateDto>> GetMailboxDelegatesAsync(Guid tenantId, string mailboxId, CancellationToken cancellationToken = default);
    Task AddMailboxDelegateAsync(Guid tenantId, string mailboxId, AddMailboxDelegateDto delegateDto, CancellationToken cancellationToken = default);
    Task RemoveMailboxDelegateAsync(Guid tenantId, string mailboxId, RemoveMailboxDelegateDto delegateDto, CancellationToken cancellationToken = default);
}
