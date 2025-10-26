using CIPP.Shared.DTOs.SharePoint;

namespace CIPP.Api.Modules.SharePoint.Interfaces;

public interface ITeamsService {
    Task<List<TeamDto>> GetTeamsAsync(Guid tenantId, CancellationToken cancellationToken = default);
    Task<TeamDetailsDto?> GetTeamDetailsAsync(Guid tenantId, string teamId, CancellationToken cancellationToken = default);
    Task<string> CreateTeamAsync(Guid tenantId, CreateTeamDto createDto, CancellationToken cancellationToken = default);
    Task<List<TeamsActivityDto>> GetTeamsActivityAsync(Guid tenantId, string type, CancellationToken cancellationToken = default);
    Task<List<TeamsVoiceDto>> GetTeamsVoiceAsync(Guid tenantId, CancellationToken cancellationToken = default);
    Task<string> AssignPhoneNumberAsync(Guid tenantId, AssignTeamsPhoneNumberDto assignDto, CancellationToken cancellationToken = default);
    Task<string> RemovePhoneNumberAsync(Guid tenantId, RemoveTeamsPhoneNumberDto removeDto, CancellationToken cancellationToken = default);
}
