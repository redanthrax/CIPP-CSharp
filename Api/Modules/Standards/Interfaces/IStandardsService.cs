using CIPP.Api.Modules.Standards.Models;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Standards;

namespace CIPP.Api.Modules.Standards.Interfaces;

public interface IStandardsService {
    Task<PagedResponse<StandardTemplateDto>> GetTemplatesAsync(string? type, string? category, PagingParameters pagingParams, CancellationToken cancellationToken = default);
    Task<StandardTemplateDto?> GetTemplateAsync(Guid templateId, CancellationToken cancellationToken = default);
    Task<StandardTemplateDto> CreateTemplateAsync(CreateStandardTemplateDto createDto, string? createdBy, CancellationToken cancellationToken = default);
    Task<StandardTemplateDto?> UpdateTemplateAsync(Guid templateId, UpdateStandardTemplateDto updateDto, string? modifiedBy, CancellationToken cancellationToken = default);
    Task<bool> DeleteTemplateAsync(Guid templateId, CancellationToken cancellationToken = default);
    Task<PagedResponse<StandardExecutionDto>> GetExecutionHistoryAsync(Guid? templateId, Guid? tenantId, PagingParameters pagingParams, CancellationToken cancellationToken = default);
    Task<List<StandardResultDto>> DeployStandardAsync(DeployStandardDto deployDto, string? executedBy, CancellationToken cancellationToken = default);
}
