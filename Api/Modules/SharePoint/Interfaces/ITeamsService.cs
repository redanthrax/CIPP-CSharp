using CIPP.Shared.DTOs.SharePoint;

namespace CIPP.Api.Modules.SharePoint.Interfaces;

public interface ITeamsService {
    Task<List<TeamDto>> GetTeamsAsync(string tenantId, CancellationToken cancellationToken = default);
    Task<TeamDetailsDto?> GetTeamDetailsAsync(string tenantId, string teamId, CancellationToken cancellationToken = default);
    Task<string> CreateTeamAsync(string tenantId, CreateTeamDto createDto, CancellationToken cancellationToken = default);
    Task<List<TeamsActivityDto>> GetTeamsActivityAsync(string tenantId, string type, CancellationToken cancellationToken = default);
    Task<List<TeamsVoiceDto>> GetTeamsVoiceAsync(string tenantId, CancellationToken cancellationToken = default);
    Task<string> AssignPhoneNumberAsync(string tenantId, AssignTeamsPhoneNumberDto assignDto, CancellationToken cancellationToken = default);
    Task<string> RemovePhoneNumberAsync(string tenantId, RemoveTeamsPhoneNumberDto removeDto, CancellationToken cancellationToken = default);
}
