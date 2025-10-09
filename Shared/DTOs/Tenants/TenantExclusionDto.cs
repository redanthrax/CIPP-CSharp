namespace CIPP.Shared.DTOs.Tenants;

public record TenantExclusionDto(
    List<Guid> TenantIds,
    bool AddExclusion
);