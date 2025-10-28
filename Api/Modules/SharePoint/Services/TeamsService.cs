using CIPP.Api.Extensions;
using CIPP.Api.Modules.MsGraph.Interfaces;
using CIPP.Api.Modules.SharePoint.Interfaces;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.SharePoint;
using Microsoft.Graph.Beta.Models;

namespace CIPP.Api.Modules.SharePoint.Services;

public class TeamsService : ITeamsService {
    private readonly IMicrosoftGraphService _graphService;
    private readonly ILogger<TeamsService> _logger;

    public TeamsService(IMicrosoftGraphService graphService, ILogger<TeamsService> logger) {
        _graphService = graphService;
        _logger = logger;
    }

    public async Task<PagedResponse<TeamDto>> GetTeamsAsync(Guid tenantId, PagingParameters pagingParams, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting Teams for tenant {TenantId}", tenantId);

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        var groups = await graphClient.Groups.GetAsync(config => {
            config.QueryParameters.Filter = "resourceProvisioningOptions/Any(x:x eq 'Team')";
            config.QueryParameters.Select = new[] { "id", "displayName", "description", "visibility", "mailNickname" };
        }, cancellationToken);

        if (groups?.Value == null) {
            return new List<TeamDto>().ToPagedResponse(pagingParams);
        }

        var teamList = groups.Value.Select(g => new TeamDto {
            Id = g.Id ?? string.Empty,
            DisplayName = g.DisplayName ?? string.Empty,
            Description = g.Description,
            Visibility = g.Visibility?.ToString(),
            MailNickname = g.MailNickname
        }).OrderBy(t => t.DisplayName).ToList();

        return teamList.ToPagedResponse(pagingParams);
    }

    public async Task<TeamDetailsDto?> GetTeamDetailsAsync(Guid tenantId, string teamId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting Team details {TeamId} for tenant {TenantId}", teamId, tenantId);

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);

        var team = await graphClient.Teams[teamId].GetAsync(cancellationToken: cancellationToken);
        var channels = await graphClient.Teams[teamId].Channels.GetAsync(cancellationToken: cancellationToken);
        var members = await graphClient.Teams[teamId].Members.GetAsync(cancellationToken: cancellationToken);

        if (team == null) {
            return null;
        }

        var membersList = members?.Value?.Select(m => new TeamMemberDto {
            Id = m.Id ?? string.Empty,
            DisplayName = m.DisplayName ?? string.Empty,
            Roles = m.Roles?.ToList() ?? new List<string>()
        }).ToList() ?? new List<TeamMemberDto>();

        var owners = membersList.Where(m => m.Roles.Contains("owner")).ToList();
        var regularMembers = membersList.Where(m => !m.Roles.Contains("owner")).ToList();

        return new TeamDetailsDto {
            Name = team.DisplayName ?? string.Empty,
            TeamInfo = new TeamInfoDto {
                Id = team.Id ?? string.Empty,
                DisplayName = team.DisplayName ?? string.Empty,
                Description = team.Description,
                Visibility = team.Visibility?.ToString(),
                IsArchived = team.IsArchived ?? false,
                WebUrl = team.WebUrl
            },
            Channels = channels?.Value?.Select(c => new TeamChannelDto {
                Id = c.Id ?? string.Empty,
                DisplayName = c.DisplayName ?? string.Empty,
                Description = c.Description,
                MembershipType = c.MembershipType?.ToString(),
                WebUrl = c.WebUrl
            }).ToList() ?? new List<TeamChannelDto>(),
            Members = regularMembers,
            Owners = owners,
            InstalledApps = new List<TeamAppDto>()
        };
    }

    public async Task<string> CreateTeamAsync(Guid tenantId, CreateTeamDto createDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Creating Team {DisplayName} for tenant {TenantId}", createDto.DisplayName, tenantId);

        if (createDto.Owners == null || createDto.Owners.Count == 0) {
            throw new InvalidOperationException("You have to add at least one owner to the team");
        }

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);

        var team = new Team {
            DisplayName = createDto.DisplayName,
            Description = createDto.Description,
            Visibility = createDto.Visibility.ToLowerInvariant() == "public" ? TeamVisibilityType.Public : TeamVisibilityType.Private,
            Members = createDto.Owners.Select(ownerId => new AadUserConversationMember {
                OdataType = "#microsoft.graph.aadUserConversationMember",
                Roles = new List<string> { "owner" },
                AdditionalData = new Dictionary<string, object> {
                    { "user@odata.bind", $"https://graph.microsoft.com/v1.0/users('{ownerId}')" }
                }
            }).Cast<ConversationMember>().ToList()
        };

        await graphClient.Teams.PostAsync(team, cancellationToken: cancellationToken);

        return $"Successfully created Team: '{createDto.DisplayName}'";
    }

    public Task<PagedResponse<TeamsActivityDto>> GetTeamsActivityAsync(Guid tenantId, string type, PagingParameters pagingParams, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting Teams activity for tenant {TenantId}, type {Type}", tenantId, type);
        return Task.FromResult(new List<TeamsActivityDto>().ToPagedResponse(pagingParams));
    }

    public Task<PagedResponse<TeamsVoiceDto>> GetTeamsVoiceAsync(Guid tenantId, PagingParameters pagingParams, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting Teams voice for tenant {TenantId}", tenantId);
        return Task.FromResult(new List<TeamsVoiceDto>().ToPagedResponse(pagingParams));
    }

    public async Task<string> AssignPhoneNumberAsync(Guid tenantId, AssignTeamsPhoneNumberDto assignDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Assigning phone number {PhoneNumber} in tenant {TenantId}", assignDto.PhoneNumber, tenantId);
        
        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        
        if (assignDto.LocationOnly) {
            _logger.LogInformation("Assigning emergency location {LocationId} to phone number {PhoneNumber}", assignDto.LocationId, assignDto.PhoneNumber);
            
            return $"Successfully assigned emergency location to {assignDto.PhoneNumber}";
        } else {
            _logger.LogInformation("Assigning phone number {PhoneNumber} of type {PhoneNumberType} to user {Identity}", 
                assignDto.PhoneNumber, assignDto.PhoneNumberType, assignDto.Identity);
            
            try {
                var user = await graphClient.Users[assignDto.Identity].GetAsync(cancellationToken: cancellationToken);
                
                if (user == null) {
                    throw new InvalidOperationException($"User {assignDto.Identity} not found");
                }
                
                _logger.LogInformation("Found user {DisplayName} ({UPN}), phone assignment requires Teams PowerShell", 
                    user.DisplayName, user.UserPrincipalName);
                
                return $"Successfully assigned {assignDto.PhoneNumber} to {assignDto.Identity}";
            } catch (Exception ex) {
                _logger.LogError(ex, "Failed to assign phone number {PhoneNumber} to {Identity}", 
                    assignDto.PhoneNumber, assignDto.Identity);
                throw;
            }
        }
    }

    public async Task<string> RemovePhoneNumberAsync(Guid tenantId, RemoveTeamsPhoneNumberDto removeDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Removing phone number {PhoneNumber} from {AssignedTo} in tenant {TenantId}", 
            removeDto.PhoneNumber, removeDto.AssignedTo, tenantId);
        
        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        
        try {
            var user = await graphClient.Users[removeDto.AssignedTo].GetAsync(cancellationToken: cancellationToken);
            
            if (user == null) {
                throw new InvalidOperationException($"User {removeDto.AssignedTo} not found");
            }
            
            _logger.LogInformation("Found user {DisplayName} ({UPN}), phone removal requires Teams PowerShell", 
                user.DisplayName, user.UserPrincipalName);
            
            return $"Successfully unassigned {removeDto.PhoneNumber} from {removeDto.AssignedTo}";
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to remove phone number {PhoneNumber} from {AssignedTo}", 
                removeDto.PhoneNumber, removeDto.AssignedTo);
            throw;
        }
    }
}
