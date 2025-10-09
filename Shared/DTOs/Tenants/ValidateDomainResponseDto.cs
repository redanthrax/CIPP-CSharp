namespace CIPP.Shared.DTOs.Tenants;

public record ValidateDomainResponseDto(
    bool Success,
    string Message,
    bool IsAvailable
);