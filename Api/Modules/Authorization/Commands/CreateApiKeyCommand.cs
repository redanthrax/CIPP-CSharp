using CIPP.Api.Modules.Authorization.Models;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Authorization.Commands;

public record CreateApiKeyCommand(
    string Name,
    string? Description,
    string CreatedBy,
    DateTime? ExpiresAt = null,
    List<Guid>? RoleIds = null
) : IRequest<CreateApiKeyCommand, Task<(ApiKey Entity, string GeneratedKey)>>;
