namespace CIPP.Shared.DTOs.Tenants;

public record TenantGroupAssignmentDto(
    Guid GroupId,
    string? GroupName = null
);