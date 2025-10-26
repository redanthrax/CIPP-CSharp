using CIPP.Api.Modules.Identity.Interfaces;
using CIPP.Api.Modules.MsGraph.Interfaces;
using CIPP.Api.Modules.MsGraph.Services;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Identity;
using Microsoft.Graph.Beta.Models;
using Microsoft.AspNetCore.WebUtilities;
using GraphServiceClient = Microsoft.Graph.Beta.GraphServiceClient;

namespace CIPP.Api.Modules.Identity.Services;

public class GroupService : IGroupService {
    private readonly GraphGroupService _graphGroupService;
    private readonly IMicrosoftGraphService _graphService;
    private readonly ILogger<GroupService> _logger;

    public GroupService(
        GraphGroupService graphGroupService,
        IMicrosoftGraphService graphService, 
        ILogger<GroupService> logger) {
        _graphGroupService = graphGroupService;
        _graphService = graphService;
        _logger = logger;
    }

    public async Task<PagedResponse<GroupDto>> GetGroupsAsync(Guid tenantId, PagingParameters? paging = null, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting groups for tenant {TenantId}", tenantId);
        
        paging ??= new PagingParameters();
        var response = await _graphGroupService.ListGroupsAsync(tenantId, paging: paging);
        
        if (response?.Value == null) {
            return new PagedResponse<GroupDto> {
                Items = new List<GroupDto>(),
                TotalCount = 0,
                PageNumber = paging.PageNumber,
                PageSize = paging.PageSize
            };
        }

        var groups = new List<GroupDto>();
        foreach (var group in response.Value) {
            var groupDto = await MapToGroupDtoAsync(group, tenantId);
            groups.Add(groupDto);
        }

        return new PagedResponse<GroupDto> {
            Items = groups,
            TotalCount = (int)(response.OdataCount ?? groups.Count),
            PageNumber = paging.PageNumber,
            PageSize = paging.PageSize,
            SkipToken = response.OdataNextLink != null ? ExtractSkipToken(response.OdataNextLink) : null
        };
    }
    
    private static string? ExtractSkipToken(string? nextLink) {
        if (string.IsNullOrEmpty(nextLink)) return null;
        var uri = new Uri(nextLink);
        var query = QueryHelpers.ParseQuery(uri.Query);
        return query.TryGetValue("$skiptoken", out var token) ? token.ToString() : null;
    }

    public async Task<GroupDto?> GetGroupAsync(Guid tenantId, string groupId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting group {GroupId} for tenant {TenantId}", groupId, tenantId);
        
        var group = await _graphGroupService.GetGroupAsync(groupId, tenantId);
        
        if (group == null) {
            return null;
        }

        return await MapToGroupDtoAsync(group, tenantId);
    }

    public async Task<GroupDto> CreateGroupAsync(CreateGroupDto createGroupDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Creating group {DisplayName} for tenant {TenantId}", 
            createGroupDto.DisplayName, createGroupDto.TenantId);

        var graphClient = await _graphService.GetGraphServiceClientAsync(createGroupDto.TenantId);
        
        var group = new Group {
            DisplayName = createGroupDto.DisplayName,
            Description = createGroupDto.Description,
            MailNickname = createGroupDto.MailNickname,
            GroupTypes = createGroupDto.GroupTypes,
            SecurityEnabled = createGroupDto.SecurityEnabled,
            MailEnabled = createGroupDto.MailEnabled,
            Visibility = createGroupDto.Visibility
        };

        var createdGroup = await _graphGroupService.CreateGroupAsync(group, createGroupDto.TenantId);
        
        if (createdGroup == null) {
            throw new InvalidOperationException("Failed to create group");
        }

        if (createGroupDto.Owners != null && createdGroup.Id != null) {
            foreach (var ownerId in createGroupDto.Owners) {
                await _graphGroupService.AddGroupOwnerAsync(createdGroup.Id, ownerId, createGroupDto.TenantId);
            }
        }

        if (createGroupDto.Members != null && createdGroup.Id != null) {
            foreach (var memberId in createGroupDto.Members) {
                await _graphGroupService.AddGroupMemberAsync(createdGroup.Id, memberId, createGroupDto.TenantId);
            }
        }

        return await MapToGroupDtoAsync(createdGroup, createGroupDto.TenantId);
    }

    public async Task<GroupDto> UpdateGroupAsync(Guid tenantId, string groupId, UpdateGroupDto updateGroupDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Updating group {GroupId} for tenant {TenantId}", groupId, tenantId);

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        
        var group = new Group();
        
        if (updateGroupDto.DisplayName != null) group.DisplayName = updateGroupDto.DisplayName;
        if (updateGroupDto.Description != null) group.Description = updateGroupDto.Description;
        if (updateGroupDto.Visibility != null) group.Visibility = updateGroupDto.Visibility;

        var updatedGroup = await _graphGroupService.UpdateGroupAsync(groupId, group, tenantId);
        
        if (updatedGroup == null) {
            throw new InvalidOperationException("Failed to update group");
        }

        return await MapToGroupDtoAsync(updatedGroup, tenantId);
    }

    public async Task DeleteGroupAsync(Guid tenantId, string groupId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Deleting group {GroupId} for tenant {TenantId}", groupId, tenantId);

        await _graphGroupService.DeleteGroupAsync(groupId, tenantId);
    }

    public async Task AddGroupMemberAsync(Guid tenantId, string groupId, AddGroupMemberDto addMemberDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Adding member {UserId} to group {GroupId} for tenant {TenantId}", 
            addMemberDto.UserId, groupId, tenantId);

        if (addMemberDto.AsOwner) {
            await _graphGroupService.AddGroupOwnerAsync(groupId, addMemberDto.UserId, tenantId);
        } else {
            await _graphGroupService.AddGroupMemberAsync(groupId, addMemberDto.UserId, tenantId);
        }
    }

    public async Task RemoveGroupMemberAsync(Guid tenantId, string groupId, string userId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Removing member {UserId} from group {GroupId} for tenant {TenantId}", 
            userId, groupId, tenantId);

        try {
            await _graphGroupService.RemoveGroupMemberAsync(groupId, userId, tenantId);
        }
        catch {
            await _graphGroupService.RemoveGroupOwnerAsync(groupId, userId, tenantId);
        }
    }

    public async Task<IEnumerable<UserDto>> GetGroupMembersAsync(Guid tenantId, string groupId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting members for group {GroupId} in tenant {TenantId}", groupId, tenantId);

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        var members = await graphClient.Groups[groupId].Members.GetAsync(null, cancellationToken);
        
        var users = new List<UserDto>();
        if (members?.Value != null) {
            foreach (var member in members.Value) {
                if (member is User user) {
                    users.Add(new UserDto {
                        Id = user.Id ?? "",
                        UserPrincipalName = user.UserPrincipalName ?? "",
                        DisplayName = user.DisplayName ?? "",
                        Mail = user.Mail ?? "",
                        TenantId = tenantId
                    });
                }
            }
        }

        return users;
    }

    public async Task<IEnumerable<UserDto>> GetGroupOwnersAsync(Guid tenantId, string groupId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting owners for group {GroupId} in tenant {TenantId}", groupId, tenantId);

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        var owners = await graphClient.Groups[groupId].Owners.GetAsync(null, cancellationToken);
        
        var users = new List<UserDto>();
        if (owners?.Value != null) {
            foreach (var owner in owners.Value) {
                if (owner is User user) {
                    users.Add(new UserDto {
                        Id = user.Id ?? "",
                        UserPrincipalName = user.UserPrincipalName ?? "",
                        DisplayName = user.DisplayName ?? "",
                        Mail = user.Mail ?? "",
                        TenantId = tenantId
                    });
                }
            }
        }

        return users;
    }

    private async Task<GroupDto> MapToGroupDtoAsync(Group group, Guid tenantId) {
        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        
        var memberCount = await GetGroupMemberCountAsync(graphClient, group.Id!);
        var owners = await GetGroupOwnerNamesAsync(graphClient, group.Id!);
        var members = await GetGroupMemberNamesAsync(graphClient, group.Id!);

        return new GroupDto {
            Id = group.Id ?? "",
            DisplayName = group.DisplayName ?? "",
            Description = group.Description ?? "",
            Mail = group.Mail ?? "",
            MailNickname = group.MailNickname ?? "",
            GroupTypes = group.GroupTypes?.ToList() ?? new List<string>(),
            SecurityEnabled = group.SecurityEnabled ?? false,
            MailEnabled = group.MailEnabled ?? false,
            Visibility = group.Visibility ?? "",
            CreatedDateTime = group.CreatedDateTime?.DateTime,
            MemberCount = memberCount,
            Owners = owners,
            Members = members,
            TenantId = tenantId
        };
    }

    private static async Task<int> GetGroupMemberCountAsync(GraphServiceClient graphClient, string groupId) {
        try {
            var members = await graphClient.Groups[groupId].Members.GetAsync();
            return members?.Value?.Count ?? 0;
        }
        catch {
            return 0;
        }
    }

    private static async Task<List<string>> GetGroupOwnerNamesAsync(GraphServiceClient graphClient, string groupId) {
        try {
            var owners = await graphClient.Groups[groupId].Owners.GetAsync();
            var ownerNames = new List<string>();
            
            if (owners?.Value != null) {
                foreach (var owner in owners.Value) {
                    if (owner is User user) {
                        ownerNames.Add(user.DisplayName ?? "");
                    }
                }
            }
            
            return ownerNames;
        }
        catch {
            return new List<string>();
        }
    }

    private static async Task<List<string>> GetGroupMemberNamesAsync(GraphServiceClient graphClient, string groupId) {
        try {
            var members = await graphClient.Groups[groupId].Members.GetAsync();
            var memberNames = new List<string>();
            
            if (members?.Value != null) {
                foreach (var member in members.Value) {
                    if (member is User user) {
                        memberNames.Add(user.DisplayName ?? "");
                    }
                }
            }
            
            return memberNames;
        }
        catch {
            return new List<string>();
        }
    }

    private static async Task AddGroupOwnerAsync(GraphServiceClient graphClient, string groupId, string userId, CancellationToken cancellationToken) {
        var requestBody = new ReferenceCreate {
            OdataId = $"https://graph.microsoft.com/beta/users/{userId}"
        };
        await graphClient.Groups[groupId].Owners.Ref.PostAsync(requestBody, null, cancellationToken);
    }

    private static async Task AddGroupMemberAsync(GraphServiceClient graphClient, string groupId, string userId, CancellationToken cancellationToken) {
        var requestBody = new ReferenceCreate {
            OdataId = $"https://graph.microsoft.com/beta/users/{userId}"
        };
        await graphClient.Groups[groupId].Members.Ref.PostAsync(requestBody, null, cancellationToken);
    }
}