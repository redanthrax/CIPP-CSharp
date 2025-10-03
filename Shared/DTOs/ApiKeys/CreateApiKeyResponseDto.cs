namespace CIPP.Shared.DTOs.ApiKeys;
public record CreateApiKeyResponseDto(
    Guid Id,
    string Name,
    string ApiKey,
    string? Description,
    DateTime CreatedAt,
    string CreatedBy,
    DateTime? ExpiresAt,
    List<ApiKeyRoleDto> Roles
);