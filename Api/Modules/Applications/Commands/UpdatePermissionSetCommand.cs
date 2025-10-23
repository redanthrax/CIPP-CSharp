using CIPP.Shared.DTOs.Applications;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Applications.Commands;

public record UpdatePermissionSetCommand(
    Guid Id,
    UpdatePermissionSetDto UpdateDto
) : IRequest<UpdatePermissionSetCommand, Task<PermissionSetDto>>;
