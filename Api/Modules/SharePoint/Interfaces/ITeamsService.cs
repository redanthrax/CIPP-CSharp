using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.SharePoint;

namespace CIPP.Api.Modules.SharePoint.Interfaces;

public interface ITeamsService {
    Task<PagedResponse<TeamDto>> GetTeamsAsync(Guid tenantId, PagingParameters pagingParams, CancellationToken cancellationToken = default);
    Task<TeamDetailsDto?> GetTeamDetailsAsync(Guid tenantId, string teamId, CancellationToken cancellationToken = default);
    Task<string> CreateTeamAsync(Guid tenantId, CreateTeamDto createDto, CancellationToken cancellationToken = default);
    Task<PagedResponse<TeamsActivityDto>> GetTeamsActivityAsync(Guid tenantId, string type, PagingParameters pagingParams, CancellationToken cancellationToken = default);
    Task<PagedResponse<TeamsVoiceDto>> GetTeamsVoiceAsync(Guid tenantId, PagingParameters pagingParams, CancellationToken cancellationToken = default);
    Task<string> AssignPhoneNumberAsync(Guid tenantId, AssignTeamsPhoneNumberDto assignDto, CancellationToken cancellationToken = default);
    Task<string> RemovePhoneNumberAsync(Guid tenantId, RemoveTeamsPhoneNumberDto removeDto, CancellationToken cancellationToken = default);
}
