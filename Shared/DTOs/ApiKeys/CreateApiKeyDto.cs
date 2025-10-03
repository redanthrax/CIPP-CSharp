namespace CIPP.Shared.DTOs.ApiKeys;
public record CreateApiKeyDto(
    string Name,
    string? Description = null,
    DateTime? ExpiresAt = null,
    List<Guid>? RoleIds = null
);