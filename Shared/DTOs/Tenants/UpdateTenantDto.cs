namespace CIPP.Shared.DTOs.Tenants;

public record UpdateTenantDto(
    string? TenantAlias,
    List<TenantGroupAssignmentDto>? TenantGroups
);