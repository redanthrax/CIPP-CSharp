using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Applications;

namespace CIPP.Api.Modules.Applications.Interfaces;

public interface IAppTemplateService {
    Task<PagedResponse<AppTemplateDto>> GetTemplatesAsync(PagingParameters? paging = null, CancellationToken cancellationToken = default);
    Task<AppTemplateDto?> GetTemplateAsync(Guid id, CancellationToken cancellationToken = default);
    Task<AppTemplateDto> CreateTemplateAsync(CreateAppTemplateDto createDto, CancellationToken cancellationToken = default);
    Task<AppTemplateDto> UpdateTemplateAsync(Guid id, UpdateAppTemplateDto updateDto, CancellationToken cancellationToken = default);
    Task DeleteTemplateAsync(Guid id, CancellationToken cancellationToken = default);
}
