namespace CIPP.Shared.DTOs.Tenants;
public record TenantDto(
    Guid Id,
    string TenantId,
    string DisplayName,
    string DefaultDomainName,
    string Status,
    DateTime CreatedAt,
    Guid CreatedBy,
    string? Metadata = null,
    string? TenantAlias = null,
    bool Excluded = false,
    Guid? ExcludeUser = null,
    DateTime? ExcludeDate = null
);
