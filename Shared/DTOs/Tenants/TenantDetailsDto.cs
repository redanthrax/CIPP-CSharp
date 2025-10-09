namespace CIPP.Shared.DTOs.Tenants;

public record TenantDetailsDto(
    Guid Id,
    string TenantId,
    string DisplayName,
    string? TenantAlias,
    string DefaultDomainName,
    string? InitialDomainName,
    string Status,
    DateTime CreatedAt,
    Guid CreatedBy,
    string? Metadata,
    List<string>? DomainList,
    int GraphErrorCount,
    DateTime? LastSyncAt,
    bool Excluded,
    Guid? ExcludeUser,
    DateTime? ExcludeDate,
    string? DelegatedPrivilegeStatus,
    bool RequiresRefresh,
    string? LastGraphError,
    DateTime? LastRefresh,
    List<TenantGroupDto>? Groups,
    OrganizationDataDto? OrganizationData
);