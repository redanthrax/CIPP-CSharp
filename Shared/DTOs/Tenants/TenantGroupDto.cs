namespace CIPP.Shared.DTOs.Tenants;

public record TenantGroupDto(
    Guid Id,
    string Name,
    string? Description,
    DateTime CreatedAt,
    Guid CreatedBy,
    DateTime? UpdatedAt = null,
    Guid? UpdatedBy = null,
    List<Guid>? MemberTenantIds = null
);