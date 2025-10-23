using CIPP.Shared.DTOs.ConditionalAccess;

namespace CIPP.Api.Modules.ConditionalAccess.Interfaces;

public interface IConditionalAccessTemplateService {
    Task<List<ConditionalAccessTemplateDto>> GetTemplatesAsync(CancellationToken cancellationToken = default);
    Task<ConditionalAccessTemplateDto?> GetTemplateAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ConditionalAccessTemplateDto> CreateTemplateAsync(CreateConditionalAccessTemplateDto createDto, CancellationToken cancellationToken = default);
    Task<ConditionalAccessTemplateDto> UpdateTemplateAsync(Guid id, UpdateConditionalAccessTemplateDto updateDto, CancellationToken cancellationToken = default);
    Task DeleteTemplateAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ConditionalAccessPolicyDto> DeployTemplateAsync(DeployConditionalAccessTemplateDto deployDto, CancellationToken cancellationToken = default);
}
