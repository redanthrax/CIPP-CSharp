using CIPP.Shared.DTOs.Exchange;

namespace CIPP.Api.Modules.Exchange.Interfaces;

public interface IContactService {
    Task<List<ContactDto>> GetContactsAsync(string tenantId, CancellationToken cancellationToken = default);
    Task<ContactDetailsDto?> GetContactAsync(string tenantId, string contactId, CancellationToken cancellationToken = default);
    Task<string> CreateContactAsync(string tenantId, CreateContactDto createDto, CancellationToken cancellationToken = default);
    Task UpdateContactAsync(string tenantId, string contactId, UpdateContactDto updateDto, CancellationToken cancellationToken = default);
    Task DeleteContactAsync(string tenantId, string contactId, CancellationToken cancellationToken = default);
}
