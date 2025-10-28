using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;

namespace CIPP.Api.Modules.Exchange.Interfaces;

public interface IContactService {
    Task<PagedResponse<ContactDto>> GetContactsAsync(Guid tenantId, PagingParameters pagingParams, CancellationToken cancellationToken = default);
    Task<ContactDetailsDto?> GetContactAsync(Guid tenantId, string contactId, CancellationToken cancellationToken = default);
    Task<string> CreateContactAsync(Guid tenantId, CreateContactDto createDto, CancellationToken cancellationToken = default);
    Task UpdateContactAsync(Guid tenantId, string contactId, UpdateContactDto updateDto, CancellationToken cancellationToken = default);
    Task DeleteContactAsync(Guid tenantId, string contactId, CancellationToken cancellationToken = default);
}
