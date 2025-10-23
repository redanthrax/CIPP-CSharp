using CIPP.Api.Modules.Identity.Interfaces;
using CIPP.Api.Modules.MsGraph.Interfaces;
using CIPP.Api.Modules.MsGraph.Services;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Identity;
using Microsoft.Graph.Beta.Models;
using Microsoft.AspNetCore.WebUtilities;

namespace CIPP.Api.Modules.Identity.Services;

public class RoleService : IRoleService {
    private readonly GraphRoleService _graphRoleService;
    private readonly GraphUserService _graphUserService;
    private readonly ILogger<RoleService> _logger;

    public RoleService(GraphRoleService graphRoleService, GraphUserService graphUserService, ILogger<RoleService> logger) {
        _graphRoleService = graphRoleService;
        _graphUserService = graphUserService;
        _logger = logger;
    }

    public async Task<PagedResponse<RoleDto>> GetRolesAsync(string tenantId, PagingParameters? paging = null, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting roles for tenant {TenantId}", tenantId);
        
        paging ??= new PagingParameters();
        var roles = await _graphRoleService.GetRoleDefinitionsAsync(tenantId);
        
        if (roles?.Value == null) {
            return new PagedResponse<RoleDto> {
                Items = new List<RoleDto>(),
                TotalCount = 0,
                PageNumber = paging.PageNumber,
                PageSize = paging.PageSize
            };
        }

        var allRoles = roles.Value.Select(role => MapToRoleDto(role, tenantId)).ToList();
        var pagedRoles = allRoles.Skip(paging.Skip).Take(paging.Take).ToList();
        
        return new PagedResponse<RoleDto> {
            Items = pagedRoles,
            TotalCount = allRoles.Count,
            PageNumber = paging.PageNumber,
            PageSize = paging.PageSize
        };
    }

    public async Task<RoleDto?> GetRoleAsync(string tenantId, string roleId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting role {RoleId} for tenant {TenantId}", roleId, tenantId);
        
        var role = await _graphRoleService.GetRoleDefinitionAsync(roleId, tenantId);
        
        return role != null ? MapToRoleDto(role, tenantId) : null;
    }

    public async Task AssignRoleAsync(string tenantId, AssignRoleDto assignRoleDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Assigning role {RoleId} to user {UserId} for tenant {TenantId}", 
            assignRoleDto.RoleId, assignRoleDto.UserId, tenantId);

        var roleAssignment = new UnifiedRoleAssignment {
            PrincipalId = assignRoleDto.UserId,
            RoleDefinitionId = assignRoleDto.RoleId,
            DirectoryScopeId = "/"
        };

        await _graphRoleService.CreateRoleAssignmentAsync(roleAssignment, tenantId);
    }

    public async Task RemoveRoleAsync(string tenantId, string userId, string roleId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Removing role {RoleId} from user {UserId} for tenant {TenantId}", 
            roleId, userId, tenantId);

        var assignments = await _graphRoleService.GetRoleAssignmentsAsync(
            tenantId,
            $"principalId eq '{userId}' and roleDefinitionId eq '{roleId}'"
        );

        if (assignments?.Value != null) {
            foreach (var assignment in assignments.Value) {
                if (assignment.Id != null) {
                    await _graphRoleService.DeleteRoleAssignmentAsync(assignment.Id, tenantId);
                }
            }
        }
    }

    public async Task<IEnumerable<RoleDto>> GetUserRolesAsync(string tenantId, string userId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting roles for user {UserId} in tenant {TenantId}", userId, tenantId);

        var assignments = await _graphRoleService.GetRoleAssignmentsAsync(
            tenantId,
            $"principalId eq '{userId}'",
            new[] { "roleDefinition" }
        );

        var roles = new List<RoleDto>();
        if (assignments?.Value != null) {
            foreach (var assignment in assignments.Value) {
                if (assignment.RoleDefinition != null) {
                    roles.Add(MapToRoleDto(assignment.RoleDefinition, tenantId));
                }
            }
        }

        return roles;
    }

    public async Task<IEnumerable<UserDto>> GetRoleMembersAsync(string tenantId, string roleId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting members for role {RoleId} in tenant {TenantId}", roleId, tenantId);

        var assignments = await _graphRoleService.GetRoleAssignmentsAsync(
            tenantId,
            $"roleDefinitionId eq '{roleId}'"
        );

        var users = new List<UserDto>();
        if (assignments?.Value != null) {
            foreach (var assignment in assignments.Value) {
                if (assignment.PrincipalId != null) {
                    try {
                        var user = await _graphUserService.GetUserAsync(assignment.PrincipalId, tenantId);
                        if (user != null) {
                            users.Add(new UserDto {
                                Id = user.Id ?? "",
                                UserPrincipalName = user.UserPrincipalName ?? "",
                                DisplayName = user.DisplayName ?? "",
                                Mail = user.Mail ?? "",
                                TenantId = tenantId
                            });
                        }
                    }
                    catch {
                        continue;
                    }
                }
            }
        }

        return users;
    }

    private static RoleDto MapToRoleDto(UnifiedRoleDefinition role, string tenantId) {
        return new RoleDto {
            Id = role.Id ?? "",
            DisplayName = role.DisplayName ?? "",
            Description = role.Description ?? "",
            TemplateId = role.TemplateId ?? "",
            IsBuiltIn = role.IsBuiltIn ?? false,
            IsEnabled = role.IsEnabled ?? false,
            ResourceScopes = role.ResourceScopes?.ToList() ?? new List<string>(),
            RolePermissions = role.RolePermissions?.Select(p => new RolePermissionDto {
                Id = "",
                AllowedResourceActions = p.AllowedResourceActions?.ToList() ?? new List<string>(),
                Condition = p.Condition ?? ""
            }).ToList() ?? new List<RolePermissionDto>(),
            TenantId = tenantId
        };
    }
}