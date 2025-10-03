namespace CIPP.Shared.DTOs.ApiKeys;
public record ApiKeyDto(
    Guid Id,
    string Name,
    string? Description,
    DateTime CreatedAt,
    string CreatedBy,
    DateTime? ExpiresAt,
    DateTime? LastUsedAt,
    int UsageCount,
    List<ApiKeyRoleDto> Roles
);

public record ApiKeyRoleDto(
    Guid RoleId,
    string RoleName,
    string RoleDescription,
    DateTime AssignedAt,
    string AssignedBy,
    DateTime? ExpiresAt = null
);